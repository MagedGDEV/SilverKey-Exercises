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

    //Get requests
    static public async Task GetDictionaryOfRecipesAsync()
    {
        var msg = new HttpRequestMessage(HttpMethod.Get, s_url);
        var res = await s_client.SendAsync(msg);
        var contentString = await res.Content.ReadAsStringAsync();
        Recipes = JsonSerializer.Deserialize<Dictionary<Guid, RecipeModel>>(contentString)!;
    }

    // Post requests
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

    // Delete requests
    static public async Task DeleteRecipeIngredientsAsync(List<string> ingredients, Guid recipeId)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri(s_editUrl + $"/{recipeId}/ingredients"),
            Content = new StringContent(JsonSerializer.Serialize(ingredients), Encoding.UTF8, "application/json")
        };
        var response = await s_client.SendAsync(request);
        _ = await response.Content.ReadAsStringAsync();
        GetDictionaryOfRecipesAsync().Wait();
    }

    static public async Task DeleteRecipeInstructionsAsync(List<string> instructions, Guid recipeId)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri(s_editUrl + $"/{recipeId}/instructions"),
            Content = new StringContent(JsonSerializer.Serialize(instructions), Encoding.UTF8, "application/json")
        };
        var response = await s_client.SendAsync(request);
        _ = await response.Content.ReadAsStringAsync();
        GetDictionaryOfRecipesAsync().Wait();
    }

    static public async Task DeleteRecipeCategoriesAsync(List<string> categories, Guid recipeId)
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Delete,
            RequestUri = new Uri(s_editUrl + $"/{recipeId}/categories"),
            Content = new StringContent(JsonSerializer.Serialize(categories), Encoding.UTF8, "application/json")
        };
        var response = await s_client.SendAsync(request);
        _ = await response.Content.ReadAsStringAsync();
        GetDictionaryOfRecipesAsync().Wait();
    }

    static public async Task DeleteRecipesAsync(Guid recipeId)
    {
        string url = s_url + $"/{recipeId}";
        var response = await s_client.DeleteAsync(url);
        _ = await response.Content.ReadAsStringAsync();
        GetDictionaryOfRecipesAsync().Wait();
    }

    // Put requests
    static public async Task UpdateRecipeIngredientsAsync(string currentIngredient, string updatedIngredient, Guid recipeId)
    {
        var json = JsonSerializer.Serialize(updatedIngredient);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        string url = s_editUrl + $"/{recipeId}/ingredient/{currentIngredient}";
        var response = await s_client.PutAsync(url, data);
        _ = await response.Content.ReadAsStringAsync();
        GetDictionaryOfRecipesAsync().Wait();
    }
}


