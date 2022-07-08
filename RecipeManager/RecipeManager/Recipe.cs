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

	public void EditTitle(string newTitle)
    {
		Title = newTitle;
    }

	public void AddCategory(string category)
    {
		Categories.Add(category);
    }

	public void AddInstruction (string instruction, int index)
    {
		Instructions.Insert(index - 1, instruction);
    }

	public void AddIngredient(string ingredient, int index)
	{
		Ingredients.Insert(index - 1, ingredient);
	}

	public void EditInstruction (string oldInstruction, string newInstruction)
    {
		int index = Instructions.IndexOf(oldInstruction);
		Instructions[index] = newInstruction;
    }

	public void EditIngredients(string oldIngredients, string newIngredients)
	{
		int index = Ingredients.IndexOf(oldIngredients);
		Ingredients[index] = newIngredients;
	}

	public void DeleteIngredient(string itemToDelete)
    {
		Ingredients.Remove(itemToDelete);
    }

	public void DeleteInstructions(string itemToDelete)
    {
		Instructions.Remove(itemToDelete);
    }

	public void DeleteCategory (string itemToDelete)
    {
		Categories.Remove(itemToDelete);
    }

	public void EditCategory (string oldCategory, string newCategory)
    {
		int index = Categories.IndexOf(oldCategory);
		Categories[index] = newCategory;
	}
}