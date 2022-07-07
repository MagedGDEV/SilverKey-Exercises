public class Recipe
{
	public string Title { get; set; }
	public List<string> Ingredients { get; set; }
	public List<string> Instructions { get; set; }
	public List<string> Categories { get; set; }
	public Recipe(string title, List<string> ingredients, List<string> instructions, List<string> categories)
	{
		Title = title;
		Ingredients = new(ingredients);
		Instructions = new(instructions);
		Categories = new(categories);
	}

	public void DeleteData(string item, string listName)
	{
		listName = listName.ToUpper();
		if (listName == "INGREDIENTS")
		{
			if (Ingredients.Contains(item))
			{
				Ingredients.Remove(item);
			}
			else
			{
				//TODO: report item not in list 
			}
		}
		else if (listName == "INSTRUCTIONS")
		{
			if (Instructions.Contains(item))
			{
				Instructions.Remove(item);
			}
			else
			{
				//TODO: report item not in list 
			}
		}
		else if (listName == "CATEGORIES")
		{
			if (Categories.Contains(item))
			{
				Categories.Remove(item);
			}
			else
			{
				//TODO: report item not in list 
			}
		}
		else
		{
			//TODO: report invalid list 
		}
	}

	public void AddData(string item, string listName)
	{
		listName = listName.ToUpper();
		if (listName == "INGREDIENTS")
		{
			if (Ingredients.Contains(item))
			{
				//TODO: report item already available
			}
			else
			{
				Ingredients.Add(item);
			}
		}
		else if (listName == "INSTRUCTIONS")
		{
			if (Instructions.Contains(item))
			{
				//TODO: report item already available
			}
			else
			{
				Instructions.Add(item);
			}
		}
		else if (listName == "CATEGORIES")
		{
			if (Categories.Contains(item))
			{
				//TODO: report item already available
			}
			else
			{
				Categories.Add(item);
			}
		}
		else
		{
			//TODO: report invalid list 
		}
	}

	// for testing the above functions only
	public void PrintRecipe()
	{
		Console.WriteLine("Recipe " + Title);
		Console.WriteLine("Ingredients");
		foreach (string ingredient in Ingredients)
		{
			Console.Write(ingredient + " ");
		}
		Console.WriteLine("\nInstructions");
		foreach (string instruction in Instructions)
		{
			Console.Write(instruction + " ");
		}
		Console.WriteLine("\nCategories");
		foreach (string Category in Categories)
		{
			Console.Write(Category + " ");
		}
		Console.WriteLine("\n------------");
	}
}