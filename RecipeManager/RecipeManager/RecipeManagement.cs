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
		ReadCategories();
	}

	private void ReadCategories()
    {
		string jsonString = File.ReadAllText(CategoriesFileName);
		Categories = JsonSerializer.Deserialize<List<string>>(jsonString);
		ArgumentNullException.ThrowIfNull(Categories);
		
    }

	private void WriteCategory()
    {
		string jsonString = JsonSerializer.Serialize(Categories);
		File.WriteAllText(CategoriesFileName, jsonString);
    }

	public void Serialize()
    {
		WriteCategory();
		//TODO: add writing recipies in json file 
    }

	public bool AddCategory(string category)
    {
		ArgumentNullException.ThrowIfNull(Categories);
		if (Categories.Contains(category))
        {
			return false;
        }
		Categories.Add(category);
		return true;
    }

	public void EditCategory(string category, string updated)
    {
		ArgumentNullException.ThrowIfNull(Categories);
		int index = Categories.IndexOf(category);
		Categories[index] = updated;
    }

	public void DeleteCategory (string category)
    {
		ArgumentNullException.ThrowIfNull(Categories);
		Categories.Remove(category);
		//TODO: Remove category from Recipes
	}

	public void AddRecipe(string title, List<string> ingredients, List<string> instructions, List<string> categories)
	{
		Guid id = Guid.NewGuid();
		var recipe = new Recipe(id, title, ingredients, instructions, categories);
        Recipes.Add(id, recipe);
    }
}