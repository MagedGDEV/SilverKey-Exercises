public class RecipeModel
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
}

