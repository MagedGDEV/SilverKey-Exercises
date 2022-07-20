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
                AddCategory();
                break;
            case "Edit category":
                CategoriesAsOptions("edit");
                break;
            case "Delete category":
                CategoriesAsOptions("delete");
                break;
            case "Back":
                ConsoleViews.IntialChoices();
                break;
            default:
                ConsoleViews.ExitMessage();
                break;
        }
    }

    static private void CategoriesAsOptions(string action)
    {
        //TODO: Get the categories list from the "ConsoleViews" class
        // and display them with Back and Exit button
        // The variable action will be used to display whether this is an delete or edit
        // and which function is going to be called when a category is chosen

        CategoriesRequests.GetListOfCategoriesAsync().Wait();
        ArgumentNullException.ThrowIfNull(CategoriesRequests.Categories);
        string[] choices = { "Back", "Exit" };
        if (CategoriesRequests.Categories.Count == 0)
        {
            var userChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title($"[blue]No categories to {action}[red] please add categories[/][/]")
            .PageSize(5)
            .AddChoices(choices));
            switch (userChoice)
            {
                case "Back":
                    ConsoleViews.IntialChoices();
                    break;
                default:
                    ConsoleViews.ExitMessage();
                    break;
            }

        }
        else
        {
            var display = CategoriesRequests.Categories.ToArray();
            display = display.Concat(choices).ToArray();

            var userChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title($"[blue]Which category do you want to {action}?[/]")
            .PageSize(5)
            .AddChoices(display));
            switch (userChoice)
            {
                case "Back":
                    ConsoleViews.IntialChoices();
                    break;
                case "Exit":
                    ConsoleViews.ExitMessage();
                    break;
                default:
                    if (action == "edit")
                    {
                        EditCategory(userChoice);
                    }
                    else
                    {
                        DeleteCategory(userChoice);
                    }
                    break;
            }
        }
    }

    static private void AddCategory()
    {
        var category = AnsiConsole.Prompt(
        new TextPrompt<string>("[blue]Enter the category name:[/]")
        .PromptStyle("red")
        );
        //TODO: Http post request to add category to JSON file & add it to the existing categories list 
        // The response should be whether item is already available or successfully saved
        // Then according to the response should a certain view to notify user
        // Return to CategoriesOptions view
    }

    static private void DeleteCategory(string category)
    {
        var deleteText = new Markup($"[red]{category}[/][blue] is deleted[/]");
        AnsiConsole.Write(deleteText);
        //TODO: Http delete request to delete from the JSON file and delete it from the existing categories list
        // and from each recipe that has that category
        // Then go back the CategoriesOptions
    }

    static private void EditCategory(string category)
    {
        var updatedCategory = AnsiConsole.Prompt(
        new TextPrompt<string>($"[blue]Edit category [green]{category}[/] to:[/]")
        .PromptStyle("red")
        );
        //TODO: Http put request to update the name of category in json file and the existing categories list
        // and from each recipe that has that category
        // if successful view saved and return to CategoriesOptions
    }
}


