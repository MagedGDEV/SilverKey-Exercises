using System;
using System.Text;
using Spectre.Console;
public class RecipesViews
{

    public RecipesViews()
    {
    }

    static public void RecipeOptions()
    {
        if (ConsoleViews.s_entryPoint)
        {
            ConsoleViews.NewLine();
            ConsoleViews.s_entryPoint = false;
        }
        string[] choices = { "List recipes", "Add recipe", "Edit recipe", "Delete recipe", "Back", "Exit" };
        var userChoice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title("[blue]What are you looking for in recipe's options ?[/]")
        .PageSize(5)
        .AddChoices(choices));
        switch (userChoice)
        {
            case "List recipes":
                ListRecipes();
                break;
            case "Add recipe":
                //TODO: Add recipe view
                break;
            case "Edit recipe":
                //TODO: Edit recipe view
                break;
            case "Delete recipe":
                //TODO: Delete recipe view
                break;
            case "Back":
                ConsoleViews.IntialChoices();
                break;
            default:
                ConsoleViews.ExitMessage();
                break;
        }
    }

    static private void ListRecipes()
    {
        RecipeRequests.GetDictionaryOfRecipesAsync().Wait();
        var table = new Table();
        table.AddColumn("[red]Title[/]");
        table.AddColumn("[red]Ingredients[/]");
        table.AddColumn("[red]Instructions[/]");
        table.AddColumn("[red]Categories[/]");
        foreach (KeyValuePair<Guid, RecipeModel> recipe in RecipeRequests.Recipes)
        {
            var rowData = recipe.Value;
            table.AddRow(rowData.Title, ListView(rowData.Ingredients), ListView(rowData.Instructions), ListCategory(rowData.Categories));
        }
        AnsiConsole.Write(table);
        AnsiConsole.Write("\n");
        string[] choices = { "Back", "Exit" };
        var userChoice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title("[blue]What would you like to do now?[/]")
        .PageSize(5)
        .AddChoices(choices));
        switch (userChoice)
        {
            case "Back":
                ConsoleViews.ExitingView();
                RecipeOptions();
                break;
            default:
                ConsoleViews.ExitingView();
                ConsoleViews.ExitMessage();
                break;
        }
    }

    static private string ListView(List<string> list)
    {
        var view = new StringBuilder();
        for (int i = 0; i < list.Count; i++)
        {
            view.Append((i + 1) + "- " + list[i] + "\n");
        }
        return view.ToString();
    }

    static private string ListCategory(List<string> categories)
    {
        var view = new StringBuilder();
        for (int i = 0; i < categories.Count; i++)
        {
            view.Append(categories[i] + "\n");
        }
        return view.ToString();
    }
}


