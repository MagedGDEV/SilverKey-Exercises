using System;
using System.Text;
using System.Text.Json;

static public class CategoriesRequests
{
    static private string s_url = "https://magedgdev.azurewebsites.net/categories";
    static private HttpClient s_client = new();
    static public List<string> Categories = new();
    

    static CategoriesRequests()
    {
    }

    static public async Task GetListOfCategoriesAsync()
    {
        var msg = new HttpRequestMessage(HttpMethod.Get, s_url);
        var res = await s_client.SendAsync(msg);
        var contentString = await res.Content.ReadAsStringAsync();
        Categories = JsonSerializer.Deserialize<List<string>>(contentString)!;
        Categories.Sort();
    }

    static public async Task AddCategoryAsync(string category)
    {
        var json = JsonSerializer.Serialize(category);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await s_client.PostAsync(s_url, data);
        _ = await response.Content.ReadAsStringAsync();
        GetListOfCategoriesAsync().Wait();
    }

    static public async Task DeleteCategoryAsync (string category)
    {
        string url = s_url + $"/{category}";
        var response = await s_client.DeleteAsync(url);
        _ = await response.Content.ReadAsStringAsync();
        string[] categoryToDelete = { category };
        foreach (KeyValuePair<Guid, RecipeModel> recipe in RecipeRequests.Recipes)
        {
            if (recipe.Value.Categories.Contains(category))
            {
                RecipeRequests.DeleteRecipeCategoriesAsync(categoryToDelete.ToList(), recipe.Key).Wait();
            }
        }
        GetListOfCategoriesAsync().Wait();
    }

    static public async Task UpdateCategoryAsync (string currentCategory, string updatedCategory)
    {
        var json = JsonSerializer.Serialize(updatedCategory);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await s_client.PutAsync(s_url + $"/{currentCategory}", data);
        _ = await response.Content.ReadAsStringAsync();
        foreach (KeyValuePair<Guid, RecipeModel> recipe in RecipeRequests.Recipes)
        {
            if (recipe.Value.Categories.Contains(currentCategory))
            {
                //TODO: create edit request in recipe to edit category name 
            }
        }
        GetListOfCategoriesAsync().Wait();
    }
}


