using System;
using System.Text.Json;

static public class CategoriesRequests
{
    static private string s_url = "https://localhost:5001/categories";
    static private HttpClient s_client;
    static public List<string> Categories = new();

    static CategoriesRequests()
    {
        s_client = new();
    }

    static public async Task GetListOfCategoriesAsync()
    {
        var msg = new HttpRequestMessage(HttpMethod.Get, s_url);
        var res = await s_client.SendAsync(msg);
        var contentString = await res.Content.ReadAsStringAsync();
        Categories = JsonSerializer.Deserialize<List<string>>(contentString)!;
    }
}


