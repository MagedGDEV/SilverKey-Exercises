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

    private IResult EditRecipeTitle (Guid recipeId, [FromBody] string newTitle)
    {
        _recipes[recipeId].EditTitle(newTitle);
        WriteRecipes();
        return Results.Json(newTitle, statusCode: 200);
    }

    private IResult DeleteRecipeIngredients (Guid recipeId, [FromBody] List<string> ingredientsToDelete)
    {
        _recipes[recipeId].DeleteIngredients(ingredientsToDelete);
        WriteRecipes();
        return Results.Json(ingredientsToDelete, statusCode: 200);
    }

    private IResult AddRecipeIngredients (Guid recipeId, [FromBody] AddListModel ingredientToAdd)
    {
        _recipes[recipeId].AddIngredient(ingredientToAdd.ItemToAdd, ingredientToAdd.Position);
        WriteRecipes();
        return Results.Json(ingredientToAdd, statusCode: 200);
    }

    private IResult EditRecipeIngredients (Guid recipeId, string ingredient, [FromBody] string updatedIngredient)
    {
        _recipes[recipeId].EditIngredient(ingredient, updatedIngredient);
        WriteRecipes();
        return Results.Json(updatedIngredient, statusCode: 200);
    }

    private IResult DeleteRecipeInstructions(Guid recipeId, [FromBody] List<string> instructionsToDelete)
    {
        _recipes[recipeId].DeleteInstructions(instructionsToDelete);
        WriteRecipes();
        return Results.Json(instructionsToDelete, statusCode: 200);
    }

    private IResult AddRecipeInstructions(Guid recipeId, [FromBody] AddListModel instructionToAdd)
    {
        _recipes[recipeId].AddInstruction(instructionToAdd.ItemToAdd, instructionToAdd.Position);
        WriteRecipes();
        return Results.Json(instructionToAdd, statusCode: 200);
    }

    public void Routing(IEndpointRouteBuilder router)
    {
        router.MapGet("/recipes", ReadRecipes);
        router.MapPost("/recipes", AddRecipe);
        router.MapGet("/recipes/{recipeId}", GetRecipe);
        router.MapDelete("/recipes/{recipeId}", DeleteRecipe);
        router.MapPut("/recipe/{recipeId}/title", EditRecipeTitle);
        router.MapDelete("/recipe/{recipeId}/ingredients", DeleteRecipeIngredients);
        router.MapPost("/recipe/{recipeId}/ingredients", AddRecipeIngredients);
        router.MapPut("/recipe/{recipeId}/{ingredient}", EditRecipeIngredients);
        router.MapDelete("/recipe/{recipeId}/instructions", DeleteRecipeInstructions);
        router.MapPost("/recipe/{recipeId}/instructions", AddRecipeInstructions);

    }
}


