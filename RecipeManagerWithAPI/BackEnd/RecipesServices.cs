using System;
using System.Text.Json;

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

    public void Routing(IEndpointRouteBuilder router)
    {
        router.MapGet("/recipes", ReadRecipes);
    }
}


