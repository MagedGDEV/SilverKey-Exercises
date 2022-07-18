using System;
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

    private IResult AddCategory([FromBody]string category)
    {
        _categories.Append(category);
        var jsonString = JsonSerializer.Serialize(_categories);
        File.WriteAllText(CategoriesFileName, jsonString);
        return Results.Json(category, statusCode: 200);
    }

    public void Routing(IEndpointRouteBuilder router)
    {
        router.MapGet("/categories", ReadCategories);
        router.MapPost("/categories", AddCategory);
    }
}
