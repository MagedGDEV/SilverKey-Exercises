using System;
using System.Collections;

public class Recipie
{
	public string Title { get; set; }
	public List<string> Ingredients { get; set; }
	public List<string> Instructions { get; set; }
	public List<string> Categories { get; set; }

	public Recipie(string title, List<string> ingredients, List<string> instructions, List<string> categories)
	{
		Title = title;
		Ingredients = ingredients;
		Instructions = instructions;
		Categories = categories;
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
				// report item not in list 
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
				// report item not in list 
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
				// report item not in list 
			}
		}
		else
		{
			// report invalid list 
		}
	}

	public void AddData(string item, string listName)
	{
		listName = listName.ToUpper();
		if (listName == "INGREDIENTS")
		{
			if (Ingredients.Contains(item))
			{
				// report item already available
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
				// report item already available
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
				// report item already available
			}
			else
			{
				Categories.Add(item);
			}
		}
		else
		{
			// report invalid list 
		}
	}

	public void printRecipe()
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

