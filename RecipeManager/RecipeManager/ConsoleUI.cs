using System;
using Spectre.Console;
public class ConsoleUI
{
	private bool EntryPoint;
	RecipeManagement Manager;
	public ConsoleUI()
	{
		EntryPoint = true;
		Manager = new();
		ConsoleTitle();
		WelcomeMessage();
		WelcomeChoices();
	}

	private void ConsoleTitle()
	{
		var titleText = new FigletText("Recipe Manager").Centered().Color(Color.Blue);
		var roundedPanel = new Panel(titleText).DoubleBorder();
		AnsiConsole.Write(roundedPanel);

	}

	private void WelcomeMessage()
	{	
		AnsiConsole.Write("\n");
		var welcomeText = new Markup("[bold yellow]Welcome to Recipe Manager :stuffed_flatbread:[/]").Centered();
		AnsiConsole.Write(welcomeText);
		AnsiConsole.Write("\n");
	}

	private void ExitMessage()
	{
		Manager.Serialize();
		var goodByeMessage = new Markup("[red]GoodBye, it was nice helping you :smiling_face_with_smiling_eyes:[/]").Centered();
		AnsiConsole.Write(goodByeMessage);
	}

	private void WelcomeChoices()
	{
		if (EntryPoint)
        {
			AnsiConsole.Write("\n");
			EntryPoint = false;
		}
		string[] choices = {"Recipes", "Categories", "Exit"};
		var userChoice = AnsiConsole.Prompt(
		new SelectionPrompt<string>()
		.Title("[blue]What are you looking for to do today ?[/]")
		.PageSize(3)
		.AddChoices(choices));
		switch (userChoice)
        {
			case "Recipes":
				RecipeChoices();
				break;
			case "Categories":
				CategoryChoices();
				break;
			default:
				ExitMessage();
				break;
		}
	}

	private void RecipeChoices()
    {
		if (EntryPoint)
		{
			AnsiConsole.Write("\n");
			EntryPoint = false;
		}
		string[] choices = { "List recipes", "Add recipe", "Edit recipe", "Delete recipe","Back","Exit" };
		var userChoice = AnsiConsole.Prompt(
		new SelectionPrompt<string>()
		.Title("[blue]What are you looking for in recipe's options ?[/]")
		.PageSize(5)
		.AddChoices(choices));

		switch (userChoice)
        {
			case "List recipes":
				//TODO: view recipes
				break;
			case "Add recipe":
				//TODO: add recipes
				break;
			case "Edit recipe":
				//TODO: show recipes title as choices
				break;
			case "Delete recipe":
				//TODO: show recipes title as choices
				break;
			case "Back":
				WelcomeChoices();
				break;
			default:
				ExitMessage();
				break;
		}
	}

	private void CategoryChoices()
    {
		if (EntryPoint)
		{
			AnsiConsole.Write("\n");
			EntryPoint = false;
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
				CategoryView(true);
				break;
			case "Delete category":
				CategoryView(false);
				break;
			case "Back":
				WelcomeChoices();
				break;
			default:
				ExitMessage();
				break;
		}
	}

	private void AddCategory()
    {
		var category = AnsiConsole.Prompt(
		new TextPrompt<string>("[blue]Enter the category name:[/]")
		.PromptStyle("red")
		);
		bool added = Manager.AddCategory(category);
		if (added)
        {
			AnsiConsole.Write(new Markup("[red]saved[/]"));
		}
		else
        {
			AnsiConsole.Write(new Markup("[red]item is already available[/]"));
		}
		Thread.Sleep(1000);
		AnsiConsole.Clear();
		ReDraw(false);
	}

	private void CategoryView(bool action)
    {
		ArgumentNullException.ThrowIfNull(Manager.Categories);
		string[] choices = { "Back", "Exit" };
		if (Manager.Categories.Count == 0)
		{
			var userChoice = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
			.Title("[blue]No categories to edit[red] please add categories[/][/]")
			.PageSize(5)
			.AddChoices(choices));
			switch (userChoice)
			{
				case "Back":
					CategoryChoices();
					break;
				default:
					ExitMessage();
					break;
			}

		}
		else
		{
			var display = Manager.Categories.ToArray();
			display = display.Concat(choices).ToArray();
			var userChoice = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
			.Title("[blue]Which category do you want to edit?[/]")
			.PageSize(5)
			.AddChoices(display));
			switch (userChoice)
			{
				case "Back":
					CategoryChoices();
					break;
				case "Exit":
					ExitMessage();
					break;
				default:
					if (action)
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

	private void DeleteCategory(string category)
    {
		var deleteText = new Markup($"[red]{category}[/][blue] is deleted[/]");
		AnsiConsole.Write(deleteText);
		Manager.DeleteCategory(category);
		Thread.Sleep(1000);
		AnsiConsole.Clear();
		ReDraw(false);
	}
	private void EditCategory(string category)
    {
		var updatedCategory = AnsiConsole.Prompt(
		new TextPrompt<string>($"[blue]Edit category [green]{category}[/] to:[/]")
		.PromptStyle("red")
		);
		Manager.EditCategory(category, updatedCategory);
		AnsiConsole.Write(new Markup("[red]saved[/]"));
		Thread.Sleep(1000);
		AnsiConsole.Clear();
		ReDraw(false);
	}

	private void ReDraw(bool menu)
    {
		EntryPoint = true;
		ConsoleTitle();
		WelcomeMessage();
		if (menu)
        {
			RecipeChoices();
        }
		else
        {
			CategoryChoices();
        }
    }
}