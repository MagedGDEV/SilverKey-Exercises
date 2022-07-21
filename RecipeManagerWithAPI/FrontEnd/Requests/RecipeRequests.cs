using System;
using System.Text;
using System.Text.Json;

static public class RecipeRequests
{
    static private string s_url = "https://localhost:5001/recipes";
    static private string s_editUrl = "https://localhost:5001/recipe";
    static private HttpClient s_client = new();
    static public Dictionary<Guid, RecipeModel> Recipes = new();

    static RecipeRequests()
    {
    }

    static public async Task GetDictionaryOfRecipesAsync()
    {
        var msg = new HttpRequestMessage(HttpMethod.Get, s_url);
        var res = await s_client.SendAsync(msg);
        var contentString = await res.Content.ReadAsStringAsync();
        Recipes = JsonSerializer.Deserialize<Dictionary<Guid, RecipeModel>>(contentString)!;
    }

    static public async Task AddRecipeAsync(RecipeModel recipe)
    {
        var json = JsonSerializer.Serialize(recipe);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await s_client.PostAsync(s_url, data);
        _ = await response.Content.ReadAsStringAsync();
        GetDictionaryOfRecipesAsync().Wait();
    }

    static public async Task AddRecipeIngredientsAsync(AddListModel ingredient, Guid recipeId)
    {
        var json = JsonSerializer.Serialize(ingredient);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        string url =s_editUrl + $"/{recipeId}/ingredients";
        var response = await s_client.PostAsync(url, data);
        _ = await response.Content.ReadAsStringAsync();
        GetDictionaryOfRecipesAsync().Wait();
    }

    static public async Task AddRecipeInstructionsAsync(AddListModel instruction, Guid recipeId)
    {
        var json = JsonSerializer.Serialize(instruction);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        string url = s_editUrl + $"/{recipeId}/instructions";
        var response = await s_client.PostAsync(url, data);
        _ = await response.Content.ReadAsStringAsync();
        GetDictionaryOfRecipesAsync().Wait();
    }

    static public async Task AddRecipeCategoriesAsync(List<string> categories, Guid recipeId)
    {
        var json = JsonSerializer.Serialize(categories);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        string url = s_editUrl + $"/{recipeId}/categories";
        var response = await s_client.PostAsync(url, data);
        _ = await response.Content.ReadAsStringAsync();
        GetDictionaryOfRecipesAsync().Wait();
    }
}


