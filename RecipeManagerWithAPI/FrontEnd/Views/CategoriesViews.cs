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
        CategoriesRequests.GetListOfCategoriesAsync().Wait();
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
        var added = CategoriesRequests.Categories.Contains(category);
        if (!added)
        {
            AnsiConsole.Write(new Markup("[red]saved[/]"));
            CategoriesRequests.AddCategoryAsync(category).Wait();
        }
        else
        {
            AnsiConsole.Write(new Markup("[red]item is already available[/]"));
        }
        Thread.Sleep(ConsoleViews.SleepTime);
        ConsoleViews.ExitingView();
        CategoriesOptions();
    }

    static private void DeleteCategory(string category)
    {
        var deleteText = new Markup($"[red]{category}[/][blue] is deleted[/]");
        AnsiConsole.Write(deleteText);
        //TODO: DELETE CATEGORY HTTP
        Thread.Sleep(ConsoleViews.SleepTime);
        ConsoleViews.ExitingView();
        CategoriesOptions();
    }

    static private void EditCategory(string category)
    {
        var updatedCategory = AnsiConsole.Prompt(
        new TextPrompt<string>($"[blue]Edit category [green]{category}[/] to:[/]")
        .PromptStyle("red")
        );
        AnsiConsole.Write(new Markup("[red]saved[/]"));
        //TODO: HTTP REQUEST TO EDIT CATEGORY
        Thread.Sleep(ConsoleViews.SleepTime);
        ConsoleViews.ExitingView();
        CategoriesOptions();
    }
}


