using System;
using System.Text.Json;
public class RecipeManagement
{
	public Dictionary<Guid, Recipe> Recipes { get; set; }
	public List<string>? Categories { get; set; }
	private const string RecipesFileName = "Recipes.json";
	private const string CategoriesFileName = "Categories.json";
	public RecipeManagement()
	{
		Categories = new();
		Recipes = new();
	}

	public void ReadCategories()
    {
		string jsonString = File.ReadAllText(CategoriesFileName);
		Categories = JsonSerializer.Deserialize<List<string>>(jsonString);
		ArgumentNullException.ThrowIfNull(Categories);
		Console.Write(Categories.Count);
    }

	public void AddCategory(string category)
    {
		ArgumentNullException.ThrowIfNull(Categories);
		Categories.Add(category);
    }
}


