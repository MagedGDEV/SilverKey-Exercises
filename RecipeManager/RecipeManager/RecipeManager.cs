using System;

public class RecipeManager
{
	Dictionary<Guid, Recipe> Recipes;
	string FileName = "Data.json";
	public RecipeManager()
	{
		Recipes = new();
	}
}


