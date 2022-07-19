using System;

public class RecipesServices
{
    public Dictionary<Guid, RecipeModel> Recipes { get; set; }
    private const string RecipesFileName = "Recipes.json";
    public RecipesServices()
    {
        Recipes = new();
    }
}


