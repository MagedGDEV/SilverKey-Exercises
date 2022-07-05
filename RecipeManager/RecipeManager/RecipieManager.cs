using System;

public class RecipieManager
{
	Dictionary<Guid, Recipie> Recipies;
	string FileName = "Data.json";
	public RecipieManager()
	{
		Recipies = new();
	}
}


