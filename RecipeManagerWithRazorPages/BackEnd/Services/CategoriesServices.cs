using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

public class CategoriesServices
{
    private const string CategoriesFileName = "Categories.json";
    private List<string> _categories;
    
    public CategoriesServices()
    {
        _categories = new();
    }

    private IResult ReadCategories()
    {
        var jsonString = File.ReadAllText(CategoriesFileName);
        var categories = JsonSerializer.Deserialize<List<string>>(jsonString);
        ArgumentNullException.ThrowIfNull(categories);
        _categories = categories;
        return Results.Json(_categories, statusCode: 200);
    }

    private void WriteCategories()
    {
        var jsonString = JsonSerializer.Serialize(_categories);
         File.WriteAllText(CategoriesFileName, jsonString);
    }

    private IResult AddCategory([FromBody]string category)
    {
        _categories.Add(category);
        WriteCategories();
        //TODO: If item already available send different status code 
        return Results.Json(category, statusCode: 200);
    }

    private IResult UpdateCategory(string oldTitle,[FromBody] string newTitle)
    {
        int index = _categories.IndexOf(oldTitle);
        _categories[index] = newTitle;
        foreach (KeyValuePair<Guid, RecipeModel> recipe in RecipesServices.Recipes)
        {
            if (recipe.Value.Categories.Contains(oldTitle))
            {
                RecipesServices.Recipes[recipe.Key].EditCategory(oldTitle, newTitle);
            }
        }
        WriteCategories();
        RecipesServices.WriteRecipes();
        return Results.Json(index, statusCode: 200);
    }

    private IResult DeleteCategory(string titleToDelete)
    {
        _categories.Remove(titleToDelete);
        foreach (KeyValuePair<Guid, RecipeModel> recipe in RecipesServices.Recipes)
        {
            if (recipe.Value.Categories.Contains(titleToDelete))
            {
                RecipesServices.Recipes[recipe.Key].DeleteCategory(titleToDelete);
            }
        }
        WriteCategories();
        RecipesServices.WriteRecipes();
        return Results.Json(titleToDelete, statusCode: 200);
    }

    public void Routing(IEndpointRouteBuilder router)
    {
        router.MapGet("/categories", ReadCategories);
        router.MapPost("/categories", AddCategory);
        router.MapPut("/categories/{oldTitle}", UpdateCategory);
        router.MapDelete("/categories/{titleToDelete}", DeleteCategory);
    }
}