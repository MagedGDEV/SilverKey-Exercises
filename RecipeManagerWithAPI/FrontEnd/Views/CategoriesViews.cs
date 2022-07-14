using System;
using Spectre.Console;
public class CategoriesViews
{
    public CategoriesViews()
    {
    }

    static public void CategoriesOptions()
    {
        if (ConsoleViews.s_entryPoint)
        {
            ConsoleViews.NewLine();
            ConsoleViews.s_entryPoint = false;
        }
        string[] choices = { "Add category", "Edit category", "Delete category", "Back", "Exit" };
        var userChoice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title("[blue]What are you looking for in category's options ?[/]")
        .PageSize(5)
        .AddChoices(choices));
        switch (userChoice)
        {
            case "Add category":
                //TODO: Add category view 
                break;
            case "Edit category":
                //TODO: Edit category view 
                break;
            case "Delete category":
                //TODO: Delete category view 
                break;
            case "Back":
                ConsoleViews.IntialChoices();
                break;
            default:
                ConsoleViews.ExitMessage();
                break;
        }
    }

    private void CategoriesAsOptions()
    {
        //TODO: Get the categories list from the "ConsoleViews" class
        // and display them with Back and Exit button
    }
}


