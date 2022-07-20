using System;
using Spectre.Console;

public class ConsoleViews
{
    static public bool s_entryPoint;
    public ConsoleViews()
    {
        //TODO: Http get request to read all the recipes & categories 
        s_entryPoint = true;
        IntialPage();
    }

    static public void NewLine()
    {
        AnsiConsole.Write("\n");
    }

    static private void WelcomeMessage()
    {
        NewLine();
        var welcomeText = new Markup("[bold yellow]Welcome to Recipe Manager :stuffed_flatbread:[/]").Centered();
        AnsiConsole.Write(welcomeText);
        NewLine();
    }

    static private void ApplicationTitle()
    {
        var titleText = new FigletText("Recipe Manager").Centered().Color(Color.Blue);
        var roundedPanel = new Panel(titleText).DoubleBorder();
        AnsiConsole.Write(roundedPanel);
    }

    static public void ExitMessage()
    {
        //TODO: serialize data 
        var goodByeMessage = new Markup("[red]GoodBye, it was nice helping you :smiling_face_with_smiling_eyes:[/]").Centered();
        AnsiConsole.Write(goodByeMessage);
    }

    static public void ExitingView()
    {
        AnsiConsole.Clear();
        ApplicationTitle();
        WelcomeMessage();
        NewLine();
    }

    static public void IntialChoices()
    {
        if (s_entryPoint)
        {
            NewLine();
            s_entryPoint = false;
        }
        string[] choices = { "Recipes", "Categories", "Exit" };
        var userChoice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title("[blue]What are you looking for to do today ?[/]")
        .PageSize(3)
        .AddChoices(choices));
        switch (userChoice)
        {
            case "Recipes":
                RecipesViews.RecipeOptions();
                break;
            case "Categories":
                CategoriesViews.CategoriesOptions();
                break;
            default:
                ExitMessage();
                break;
        }
    }

    private void IntialPage()
    {
        ApplicationTitle();
        WelcomeMessage();
        IntialChoices();
    }
}


