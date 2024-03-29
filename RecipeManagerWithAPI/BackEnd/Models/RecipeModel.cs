﻿public class RecipeModel
{
    public string Title { get; set; }
    public List<string> Ingredients { get; set; }
    public List<string> Instructions { get; set; }
    public List<string> Categories { get; set; }

    public RecipeModel(string title, List<string> ingredients, List<string> instructions, List<string> categories)
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

    public void DeleteIngredients(List<string> ingredientsToDelete)
    {
        for (int i = 0; i < ingredientsToDelete.Count; i++)
        {
            Ingredients.Remove(ingredientsToDelete[i]);
        }
    }

    public void AddIngredient (string newIngredient, int position)
    {
        Ingredients.Insert(position - 1, newIngredient);
    }

    public void EditIngredient (string ingredient, string updatedIngredient)
    {
        int index = Ingredients.IndexOf(ingredient);
        Ingredients[index] = updatedIngredient;
    }

    public void DeleteInstructions(List<string> instructionsToDelete)
    {
        for (int i = 0; i < instructionsToDelete.Count; i++)
        {
            Instructions.Remove(instructionsToDelete[i]);
        }
    }

    public void AddInstruction (string newInstruction, int position)
    {
        Instructions.Insert(position - 1, newInstruction);
    }

    public void EditInstruction (string instruction, string updatedInstruction)
    {
        int index = Instructions.IndexOf(instruction);
        Instructions[index] = updatedInstruction;
    }

    public void DeleteCategories(List<string> categoriesToDelete)
    {
        for (int i = 0; i < categoriesToDelete.Count; i++)
        {
            Categories.Remove(categoriesToDelete[i]);
        }
    }

    public void AddCategories(List<string> categoriesToAdd)
    {
        for (int i = 0; i < categoriesToAdd.Count; i++)
        {
            Categories.Add(categoriesToAdd[i]);
        }
    }

    public void EditCategory(string category, string updatedCategory)
    {
        int index = Categories.IndexOf(category);
        Categories[index] = updatedCategory;
    }

    public void DeleteCategory(string itemToDelete)
    {
        Categories.Remove(itemToDelete);
    }
}

