using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Text.Json;

static public class RecipeRequests
{
    static private string s_url = "https://magedgdev.azurewebsites.net/recipes";
    static private string s_editUrl = "https://magedgdev.azurewebsites.net/recipe";
    static private string s_urlImage = "https://magedgdev.azurewebsites.net/recipesWithImages";
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

    static public async Task GetRecipesImagesAsync()
    {
        var msg = new HttpRequestMessage(HttpMethod.Get, s_urlImage);
        var res = await s_client.SendAsync(msg);
        var content = await res.Content.ReadAsStreamAsync();
        var zipPath = Path.Combine(Environment.CurrentDirectory, @"Images.zip");
        string[] fileName = Directory.GetFiles(Environment.CurrentDirectory);
        string[] directoryName = Directory.GetDirectories(Path.Combine(Environment.CurrentDirectory, @"wwwroot"));
        string imageDirectoryPath = Path.Combine(Environment.CurrentDirectory, @"wwwroot", @"recipeImages");
        if (fileName.Contains(zipPath))
        {
            File.Delete(zipPath);
        }
        if (directoryName.Contains(imageDirectoryPath))
        {
            string[] imagesNames = Directory.GetFiles(imageDirectoryPath);
            foreach (var image in imagesNames)
            {
                File.Delete(image);
            }
            Directory.Delete(imageDirectoryPath);
        }
        using (FileStream zipToOpen = new FileStream(zipPath, FileMode.Create))
        {
            using var sourceZIP = new ZipArchive(content, ZipArchiveMode.Read, false);
            using (var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
            {
                foreach (var sourceEntry in sourceZIP.Entries)
                {
                    using (Stream targetEntryStream = archive.CreateEntry(sourceEntry.FullName).Open())
                    {
                        sourceEntry.Open().CopyTo(targetEntryStream);
                    }
                }
            }
        }
        ZipFile.ExtractToDirectory(zipPath, imageDirectoryPath);
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

    static public async Task UpdateRecipeInstructionsAsync(string currentInstruction, string updatedInstruction, Guid recipeId)
    {
        var json = JsonSerializer.Serialize(updatedInstruction);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        string url = s_editUrl + $"/{recipeId}/instruction/{currentInstruction}";
        var response = await s_client.PutAsync(url, data);
        _ = await response.Content.ReadAsStringAsync();
        GetDictionaryOfRecipesAsync().Wait();
    }

    static public async Task UpdateRecipeTitleAsync(string newTitle, Guid recipeId)
    {
        var json = JsonSerializer.Serialize(newTitle);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        string url = s_editUrl + $"/{recipeId}/title";
        var response = await s_client.PutAsync(url, data);
        _ = await response.Content.ReadAsStringAsync();
        GetDictionaryOfRecipesAsync().Wait();
    }
}

//TODO: add requests for images
// update url to magedgdev...
// Get zip of images --->> done
// Delete image
// Edit image

