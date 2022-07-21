﻿using System;
using System.Text;
using System.Text.Json;

static public class CategoriesRequests
{
    static private string s_url = "https://localhost:5001/categories";
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
    }

    static public async Task AddCategoryAsync(string category)
    {
        var json = JsonSerializer.Serialize(category);
        var data = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await s_client.PostAsync(s_url, data);
        _ = await response.Content.ReadAsStringAsync();
    }
}


