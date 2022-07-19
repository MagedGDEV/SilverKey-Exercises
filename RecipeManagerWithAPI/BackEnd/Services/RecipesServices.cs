using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

public class RecipesServices
{
    private Dictionary<Guid, RecipeModel> _recipes { get; set; }
    private const string RecipesFileName = "Recipes.json";
    public RecipesServices()
    {
        _recipes = new();
    }

    private IResult ReadRecipes()
    {
        var jsonString = File.ReadAllText(RecipesFileName);
        var recipes = JsonSerializer.Deserialize<Dictionary<Guid, RecipeModel>>(jsonString);
        ArgumentNullException.ThrowIfNull(recipes);
        _recipes = recipes;
        return Results.Json(_recipes, statusCode: 200);
    }

    private void WriteRecipes()
    {
        var jsonString = JsonSerializer.Serialize(_recipes);
        File.WriteAllText(RecipesFileName, jsonString);
    }

    private IResult AddRecipe(RecipeModel recipe)
    {
        Guid id = Guid.NewGuid();
        var newRecipe = new RecipeModel(recipe.Title, recipe.Ingredients, recipe.Instructions, recipe.Categories);
        _recipes.Add(id, recipe);
        WriteRecipes();
        return Results.Json(recipe, statusCode: 200);
    }

    private IResult DeleteRecipe(Guid recipeId)
    {
        _recipes.Remove(recipeId);
        WriteRecipes();
        return Results.Json(recipeId, statusCode: 200);
    }

    private IResult GetRecipe(Guid recipeId)
    {
        return Results.Json(_recipes[recipeId], statusCode: 200);
    }

    public void Routing(IEndpointRouteBuilder router)
    {
        router.MapGet("/recipes", ReadRecipes);
        router.MapPost("/recipes", AddRecipe);
        router.MapGet("/recipes/{recipeId}", GetRecipe);
        router.MapDelete("/recipes/{recipeId}", DeleteRecipe);
    }
}


