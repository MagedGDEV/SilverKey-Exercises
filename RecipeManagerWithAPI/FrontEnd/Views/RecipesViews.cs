using System;
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
            AnsiConsole.Write("\n");
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
                //TODO: List recipe view
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
}


