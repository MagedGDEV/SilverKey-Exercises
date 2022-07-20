using System;
using System.Text.Json;

static public class RecipeRequests
{
    static private string s_url = "https://localhost:5001/recipes";
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
}


