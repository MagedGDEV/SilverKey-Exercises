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

    private async Task<IResult> ReadRecipesAsync()
    {
        var jsonString = await File.ReadAllTextAsync(RecipesFileName);
        var recipes = JsonSerializer.Deserialize<Dictionary<Guid, RecipeModel>>(jsonString);
        ArgumentNullException.ThrowIfNull(recipes);
        Recipes = recipes;
        return Results.Json(Recipes, options: options, statusCode: 200);
    }

    static public async Task WriteRecipesAsync()
    {
        var jsonString = JsonSerializer.Serialize(Recipes);
        await File.WriteAllTextAsync(RecipesFileName, jsonString);
    }

    private async Task<IResult> AddRecipeAsync(RecipeModel recipe)
    {
        Guid id = Guid.NewGuid();
        Recipes.Add(id, recipe);
        await WriteRecipesAsync();
        return Results.Json(recipe, statusCode: 200);
    }

    private async Task<IResult> AddRecipeWithImageAsync(HttpRequest request)
    {
        if (request.Form.Files.Count == 0)
        {
            
            return Results.BadRequest("This request is used when there is an image to upload");
        }
        Guid id = Guid.NewGuid();
        var type = request.Form.Files[0].ContentType.Replace("image/", ".");
        var path = Path.Combine(Environment.CurrentDirectory, @"Images", id.ToString() + type);
        using (var stream = new FileStream(path, FileMode.Create))
        {
            request.Form.Files[0].CopyTo(stream);
        }   
        var recipe = JsonSerializer.Deserialize<RecipeModel>(request.Form["recipe"], options: options)!;
        recipe.AddImage(id.ToString() + type);
        Recipes.Add(id, recipe);
        await WriteRecipesAsync();
        return Results.Ok(recipe);
    }

    private IResult ReadRecipesWithImages()
    {
        var path = Path.Combine(Environment.CurrentDirectory, @"Images");
        var zipPath = Path.Combine(Environment.CurrentDirectory, @"Images.zip");
        string[] fileName = Directory.GetFiles(Environment.CurrentDirectory);
        if (fileName.Contains(zipPath))
        {
            File.Delete(zipPath);
        }
        ZipFile.CreateFromDirectory(path, zipPath);
        return Results.File(zipPath, contentType: "application/zip");
    }

    private async Task<IResult> EditRecipeImageAsync(HttpRequest request, Guid recipeId)
    {
        if (Recipes[recipeId].ImageName != "")
        {
            var deletePath = Path.Combine(Environment.CurrentDirectory, @"Images", Recipes[recipeId].ImageName);
            File.Delete(deletePath);
        }
        var type = request.Form.Files[0].ContentType.Replace("image/", ".");
        var path = Path.Combine(Environment.CurrentDirectory, @"Images", recipeId.ToString()+type);
        using (var stream = new FileStream(path, FileMode.Create))
        {
            request.Form.Files[0].CopyTo(stream);
            Recipes[recipeId].AddImage(recipeId.ToString() + type);
            await WriteRecipesAsync();
            return Results.File(path, contentType: "image/");
        }
    }

    private async Task<IResult> DeleteRecipeImageAsync(Guid recipeId)
    {
        var path = Path.Combine(Environment.CurrentDirectory, @"Images", Recipes[recipeId].ImageName);
        if (Recipes[recipeId].ImageName != "")
        {
            File.Delete(path);
            Recipes[recipeId].ImageName = "";
            await WriteRecipesAsync();
        }
        return Results.Ok(Recipes[recipeId]);
    }

    private async Task<IResult> DeleteRecipeAsync(Guid recipeId)
    {
        var path = Path.Combine(Environment.CurrentDirectory, @"Images", Recipes[recipeId].ImageName);
        if (Recipes[recipeId].ImageName != "")
        {
            File.Delete(path);
            Recipes[recipeId].ImageName = "";
        }
        Recipes.Remove(recipeId);
        await WriteRecipesAsync();
        return Results.Json(recipeId, statusCode: 200);
    }

    private IResult GetRecipe(Guid recipeId)
    {
        return Results.Json(Recipes[recipeId], statusCode: 200);
    }

    private async Task<IResult> EditRecipeTitleAsync(Guid recipeId, [FromBody] string newTitle)
    {
        Recipes[recipeId].EditTitle(newTitle);
        await WriteRecipesAsync();
        return Results.Json(newTitle, statusCode: 200);
    }

    private async Task<IResult> DeleteRecipeIngredientsAsync(Guid recipeId, [FromBody] List<string> ingredientsToDelete)
    {
        Recipes[recipeId].DeleteIngredients(ingredientsToDelete);
        await WriteRecipesAsync();
        return Results.Json(ingredientsToDelete, statusCode: 200);
    }

    private async Task<IResult> AddRecipeIngredientsAsync(Guid recipeId, [FromBody] AddListModel ingredientToAdd)
    {
        Recipes[recipeId].AddIngredient(ingredientToAdd.ItemToAdd, ingredientToAdd.Position);
        await WriteRecipesAsync();
        return Results.Json(ingredientToAdd, statusCode: 200);
    }

    private async Task<IResult> EditRecipeIngredientsAsync(Guid recipeId, string ingredient, [FromBody] string updatedIngredient)
    {
        Recipes[recipeId].EditIngredient(ingredient, updatedIngredient);
        await WriteRecipesAsync();
        return Results.Json(updatedIngredient, statusCode: 200);
    }

    private async Task<IResult> DeleteRecipeInstructionsAsync(Guid recipeId, [FromBody] List<string> instructionsToDelete)
    {
        Recipes[recipeId].DeleteInstructions(instructionsToDelete);
        await WriteRecipesAsync();
        return Results.Json(instructionsToDelete, statusCode: 200);
    }

    private async Task<IResult> AddRecipeInstructionsAsync(Guid recipeId, [FromBody] AddListModel instructionToAdd)
    {
        Recipes[recipeId].AddInstruction(instructionToAdd.ItemToAdd, instructionToAdd.Position);
        await WriteRecipesAsync();
        return Results.Json(instructionToAdd, statusCode: 200);
    }

    private async Task<IResult> EditRecipeInstructionsAsync(Guid recipeId, string instruction, [FromBody] string updatedInstruction)
    {
        Recipes[recipeId].EditInstruction(instruction, updatedInstruction);
        await WriteRecipesAsync();
        return Results.Json(updatedInstruction, statusCode: 200);
    }

    private async Task<IResult> DeleteRecipeCategoriesAsync(Guid recipeId, [FromBody] List<string> categoriesToDelete)
    {
        Recipes[recipeId].DeleteCategories(categoriesToDelete);
        await WriteRecipesAsync();
        return Results.Json(categoriesToDelete, statusCode: 200);
    }

    private async Task<IResult> AddRecipeCategoriesAsync(Guid recipeId, [FromBody] List<string> categoriesToAdd)
    {
        Recipes[recipeId].AddCategories(categoriesToAdd);
        await WriteRecipesAsync();
        return Results.Json(categoriesToAdd, statusCode: 200);
    }

    private async Task<IResult> EditRecipeAsync(Guid recipeId, [FromBody] RecipeModel editedRecipe)
    {
        Recipes[recipeId] = editedRecipe;
        await WriteRecipesAsync();
        return Results.Json(Recipes[recipeId], statusCode: 200);
    }

    public void Routing(IEndpointRouteBuilder router)
    {
        router.MapGet("/recipes", ReadRecipesAsync);
        router.MapGet("/recipesWithImages", ReadRecipesWithImages);
        router.MapPost("/recipes", AddRecipeAsync);
        router.MapPost("/recipesWithImage", AddRecipeWithImageAsync);
        router.MapGet("/recipes/{recipeId}", GetRecipe);
        router.MapDelete("/recipes/{recipeId}", DeleteRecipeAsync);
        router.MapPut("/recipe/{recipeId}/title", EditRecipeTitleAsync);
        router.MapPut("recipe/{recipeId}", EditRecipeAsync);
        // Editing recipe's ingredients
        router.MapDelete("/recipe/{recipeId}/ingredients", DeleteRecipeIngredientsAsync);
        router.MapPost("/recipe/{recipeId}/ingredients", AddRecipeIngredientsAsync);
        router.MapPut("/recipe/{recipeId}/ingredient/{ingredient}", EditRecipeIngredientsAsync);
        // Editing recipe's instructions
        router.MapDelete("/recipe/{recipeId}/instructions", DeleteRecipeInstructionsAsync);
        router.MapPost("/recipe/{recipeId}/instructions", AddRecipeInstructionsAsync);
        router.MapPut("/recipe/{recipeId}/instruction/{instruction}", EditRecipeInstructionsAsync);
        // Editing recipe's categories
        router.MapDelete("/recipe/{recipeId}/categories", DeleteRecipeCategoriesAsync);
        router.MapPost("/recipe/{recipeId}/categories", AddRecipeCategoriesAsync);
        // Editing recipe's images
        router.MapPut("/recipe/{recipeId}/image", EditRecipeImageAsync);
        router.MapDelete("/recipe/{recipeId}/image", DeleteRecipeImageAsync);
    }
}