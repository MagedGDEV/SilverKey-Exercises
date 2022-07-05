using System;

public class RecipeManagement
{
	Dictionary<Guid, Recipe> Recipes { get; set; }
	string FileName = "Data.json";
	public RecipeManagement()
	{
		Recipes = new();
	}
}


