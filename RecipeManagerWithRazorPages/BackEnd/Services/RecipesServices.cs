using System;
using System.Text.Json;
using System.IO.Compression;
using Microsoft.AspNetCore.Mvc;

public class RecipesServices
{
    static public Dictionary<Guid, RecipeModel> Recipes = new();
    private const string RecipesFileName = "Recipes.json";
    private JsonSerializerOptions options = new JsonSerializerOptions
    {
        DictionaryKeyPolicy = null
    };

    public RecipesServices()
    {
    }

    private IResult ReadRecipes()
    {
        var jsonString = File.ReadAllText(RecipesFileName);
        var recipes = JsonSerializer.Deserialize<Dictionary<Guid, RecipeModel>>(jsonString);
        ArgumentNullException.ThrowIfNull(recipes);
        Recipes = recipes;
        return Results.Json(Recipes, options: options, statusCode: 200);
    }

    static public void WriteRecipes()
    {
        var jsonString = JsonSerializer.Serialize(Recipes);
        File.WriteAllText(RecipesFileName, jsonString);
    }

    private IResult AddRecipe(RecipeModel recipe)
    {
        Guid id = Guid.NewGuid();
        Recipes.Add(id, recipe);
        WriteRecipes();
        return Results.Json(recipe, statusCode: 200);
    }

    private IResult AddRecipeWithImage(HttpRequest request)
    {
        if (request.Form.Files.Count == 0)
        {
            
            return Results.BadRequest("This request is used when there is an image to upload");
        }
        Guid id = Guid.NewGuid();
        var path = Path.Combine(Environment.CurrentDirectory, @"Images", id.ToString());
        var stream = new FileStream(path, FileMode.Create);
        request.Form.Files[0].CopyTo(stream);
        var recipe = JsonSerializer.Deserialize<RecipeModel>(request.Form["recipe"], options: options)!;
        recipe.AddImage(id);
        Recipes.Add(id, recipe);
        WriteRecipes();
        return Results.Ok(recipe);
    }

    private IResult ReadRecipesWithImages()
    {
        var path = Path.Combine(Environment.CurrentDirectory, @"Images");
        var zipPath = Path.Combine(Environment.CurrentDirectory, @"Images.zip");
        File.Delete(zipPath);
        ZipFile.CreateFromDirectory(path, zipPath);
        string[] files = Directory.GetFiles(path);
        return Results.File(zipPath);
    }

    private IResult EditRecipeImage(HttpRequest request, Guid recipeId)
    {
        var path = Path.Combine(Environment.CurrentDirectory, @"Images", recipeId.ToString());
        File.Delete(path);
        var stream = new FileStream(path, FileMode.Create);
        request.Form.Files[0].CopyTo(stream);
        Recipes[recipeId].AddImage(recipeId);
        WriteRecipes();
        return Results.File(path);
    }

    private IResult DeleteRecipeImage(Guid recipeId)
    {
        var path = Path.Combine(Environment.CurrentDirectory, @"Images", recipeId.ToString());
        File.Delete(path);
        Recipes[recipeId].ImageName = "";
        WriteRecipes();
        return Results.Ok(Recipes[recipeId]);
    }

    private IResult DeleteRecipe(Guid recipeId)
    {
        Recipes.Remove(recipeId);
        WriteRecipes();
        return Results.Json(recipeId, statusCode: 200);
    }

    private IResult GetRecipe(Guid recipeId)
    {
        return Results.Json(Recipes[recipeId], statusCode: 200);
    }

    private IResult EditRecipeTitle(Guid recipeId, [FromBody] string newTitle)
    {
        Recipes[recipeId].EditTitle(newTitle);
        WriteRecipes();
        return Results.Json(newTitle, statusCode: 200);
    }

    private IResult DeleteRecipeIngredients(Guid recipeId, [FromBody] List<string> ingredientsToDelete)
    {
        Recipes[recipeId].DeleteIngredients(ingredientsToDelete);
        WriteRecipes();
        return Results.Json(ingredientsToDelete, statusCode: 200);
    }

    private IResult AddRecipeIngredients(Guid recipeId, [FromBody] AddListModel ingredientToAdd)
    {
        Recipes[recipeId].AddIngredient(ingredientToAdd.ItemToAdd, ingredientToAdd.Position);
        WriteRecipes();
        return Results.Json(ingredientToAdd, statusCode: 200);
    }

    private IResult EditRecipeIngredients(Guid recipeId, string ingredient, [FromBody] string updatedIngredient)
    {
        Recipes[recipeId].EditIngredient(ingredient, updatedIngredient);
        WriteRecipes();
        return Results.Json(updatedIngredient, statusCode: 200);
    }

    private IResult DeleteRecipeInstructions(Guid recipeId, [FromBody] List<string> instructionsToDelete)
    {
        Recipes[recipeId].DeleteInstructions(instructionsToDelete);
        WriteRecipes();
        return Results.Json(instructionsToDelete, statusCode: 200);
    }

    private IResult AddRecipeInstructions(Guid recipeId, [FromBody] AddListModel instructionToAdd)
    {
        Recipes[recipeId].AddInstruction(instructionToAdd.ItemToAdd, instructionToAdd.Position);
        WriteRecipes();
        return Results.Json(instructionToAdd, statusCode: 200);
    }

    private IResult EditRecipeInstructions(Guid recipeId, string instruction, [FromBody] string updatedInstruction)
    {
        Recipes[recipeId].EditInstruction(instruction, updatedInstruction);
        WriteRecipes();
        return Results.Json(updatedInstruction, statusCode: 200);
    }

    private IResult DeleteRecipeCategories(Guid recipeId, [FromBody] List<string> categoriesToDelete)
    {
        Recipes[recipeId].DeleteCategories(categoriesToDelete);
        WriteRecipes();
        return Results.Json(categoriesToDelete, statusCode: 200);
    }

    private IResult AddRecipeCategories(Guid recipeId, [FromBody] List<string> categoriesToAdd)
    {
        Recipes[recipeId].AddCategories(categoriesToAdd);
        WriteRecipes();
        return Results.Json(categoriesToAdd, statusCode: 200);
    }

    public void Routing(IEndpointRouteBuilder router)
    {
        router.MapGet("/recipes", ReadRecipes);
        router.MapGet("/recipesWithImages", ReadRecipesWithImages);
        router.MapPost("/recipes", AddRecipe);
        router.MapPost("/recipesWithImage", AddRecipeWithImage);
        router.MapGet("/recipes/{recipeId}", GetRecipe);
        router.MapDelete("/recipes/{recipeId}", DeleteRecipe);
        router.MapPut("/recipe/{recipeId}/title", EditRecipeTitle);
        // Editing recipe's ingredients
        router.MapDelete("/recipe/{recipeId}/ingredients", DeleteRecipeIngredients);
        router.MapPost("/recipe/{recipeId}/ingredients", AddRecipeIngredients);
        router.MapPut("/recipe/{recipeId}/ingredient/{ingredient}", EditRecipeIngredients);
        // Editing recipe's instructions
        router.MapDelete("/recipe/{recipeId}/instructions", DeleteRecipeInstructions);
        router.MapPost("/recipe/{recipeId}/instructions", AddRecipeInstructions);
        router.MapPut("/recipe/{recipeId}/instruction/{instruction}", EditRecipeInstructions);
        // Editing recipe's categories
        router.MapDelete("/recipe/{recipeId}/categories", DeleteRecipeCategories);
        router.MapPost("/recipe/{recipeId}/categories", AddRecipeCategories);
        // Editing recipe's images
        router.MapPut("/recipe/{recipeId}/image", EditRecipeImage);
        router.MapDelete("/recipe/{recipeId}/image", DeleteRecipeImage);
    }
}