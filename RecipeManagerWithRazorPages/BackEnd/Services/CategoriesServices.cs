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

    private async Task<IResult> ReadCategoriesAsync()
    {
        var jsonString = await File.ReadAllTextAsync(CategoriesFileName);
        var categories = JsonSerializer.Deserialize<List<string>>(jsonString);
        ArgumentNullException.ThrowIfNull(categories);
        _categories = categories;
        return Results.Json(_categories, statusCode: 200);
    }

    private async Task WriteCategoriesAsync()
    {
        var jsonString = JsonSerializer.Serialize(_categories);
         await File.WriteAllTextAsync(CategoriesFileName, jsonString);
    }

    private async Task<IResult> AddCategoryAsync([FromBody]string category)
    {
        _categories.Add(category);
        await WriteCategoriesAsync();
        return Results.Json(category, statusCode: 200);
    }

    private async Task<IResult> UpdateCategoryAsync(string oldTitle,[FromBody] string newTitle)
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
        await WriteCategoriesAsync();
        await RecipesServices.WriteRecipesAsync();
        return Results.Json(index, statusCode: 200);
    }

    private async Task<IResult> DeleteCategoryAsync(string titleToDelete)
    {
        _categories.Remove(titleToDelete);
        foreach (KeyValuePair<Guid, RecipeModel> recipe in RecipesServices.Recipes)
        {
            if (recipe.Value.Categories.Contains(titleToDelete))
            {
                RecipesServices.Recipes[recipe.Key].DeleteCategory(titleToDelete);
            }
        }
        await WriteCategoriesAsync();
        await RecipesServices.WriteRecipesAsync();
        return Results.Json(titleToDelete, statusCode: 200);
    }

    public void Routing(IEndpointRouteBuilder router)
    {
        router.MapGet("/categories", ReadCategoriesAsync);
        router.MapPost("/categories", AddCategoryAsync);
        router.MapPut("/categories/{oldTitle}", UpdateCategoryAsync);
        router.MapDelete("/categories/{titleToDelete}", DeleteCategoryAsync);
    }
}