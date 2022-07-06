using Spectre.Console;
using System.Text;
public class ConsoleUI
{
	private bool EntryPoint;
	private RecipeManagement Manager;
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
				ListRecipe();
				break;
			case "Add recipe":
				AddRecipe();
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


	private List<string> RecipeQuestions (string listName)
    {
		List<string> inputList = new();
		var questionText = new Markup($"[blue]What are the [green]{listName}[/] of the recipe?[/]");
		AnsiConsole.Write(questionText);
		AnsiConsole.Write("\n");
        while (true)
        {
			string input = AnsiConsole.Prompt(new TextPrompt<string>("").PromptStyle("red"));
			if (input.ToUpper() == "DONE")
				break;
			inputList.Add(input);
        }
		return inputList;
    }

	private void ListRecipe()
    {
		var table = new Table();
		table.AddColumn("Title");
		table.AddColumn("Ingredients");
		table.AddColumn("Instructions");
		table.AddColumn("Categories");
		foreach (KeyValuePair<Guid, Recipe> recipe in Manager.Recipes)
		{
			var rowData = recipe.Value;
			table.AddRow(rowData.Title, ListView(rowData.Ingredients), ListView(rowData.Instructions), ListCategory(rowData.Categories));
		}
		AnsiConsole.Write(table);
		AnsiConsole.Write("\n");
		string[] choices = { "Back", "Exit" };
		var userChoice = AnsiConsole.Prompt(
		new SelectionPrompt<string>()
		.Title($"[blue]What would you like to do now?[/]")
		.PageSize(5)
		.AddChoices(choices));
		switch (userChoice)
		{
			case "Back":
				AnsiConsole.Clear();
				ReDraw(true);
				break;
			default:
				AnsiConsole.Clear();
				ConsoleTitle();
				WelcomeMessage();
				AnsiConsole.Write("\n");
				ExitMessage();
				break;
		}		
	}

	private string ListView(List<string> list)
    {
		var view = new StringBuilder();
		for (int i = 0; i < list.Count; i++)
        {
			view.Append((i+1)+"- "+list[i]+"\n");
        }
		return view.ToString();
    }

	private string ListCategory(List <string> categories)
    {
		var view = new StringBuilder();
		for (int i = 0; i < categories.Count; i++)
		{
			view.Append(categories[i] + "\n");
		}
		return view.ToString();
	}

	private void AddRecipe()
    {
		ArgumentNullException.ThrowIfNull(Manager.Categories);
		if (Manager.Categories.Count == 0)
        {
			string[] choices = { "Back", "Exit" };
			var userChoice = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
			.Title($"[blue]No categories available to choose for the recipe[red] please add categories first[/][/]")
			.PageSize(5)
			.AddChoices(choices));
			switch (userChoice)
			{
				case "Back":
					RecipeChoices();
					break;
				default:
					ExitMessage();
					break;
			}
		}
		else
        {
			var title = AnsiConsole.Prompt(
			new TextPrompt<string>($"\n[blue]What is the [green]title[/] of the recipe?[/]")
			.PromptStyle("red")
			);
			List<string> ingredients = RecipeQuestions("ingredients");
			List<string> instructions = RecipeQuestions("instructions");
			
			List<string> categories = AnsiConsole.Prompt(
			new MultiSelectionPrompt<string>()
			.Title("[blue]What are the [green]categories[/] of the recipe?[/]")
			.PageSize(10)
			.InstructionsText(
				"[grey](Press [blue]<space>[/] to toggle a category, " +
				"[green]<enter>[/] to accept)[/]")
			.AddChoices(Manager.Categories
			));
			Manager.AddRecipe(title, ingredients, instructions, categories);
			AnsiConsole.Clear();
			ConsoleTitle();
			WelcomeMessage();
			AnsiConsole.Write("\n");
			AnsiConsole.Write(new Markup("[red]Saved[/]"));
			Thread.Sleep(1000);
			AnsiConsole.Clear();
			ReDraw(true);
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
		string text;
		if (action)
		{
			text = "edit";
		}
		else
		{
			text = "delete";
		}
		if (Manager.Categories.Count == 0)
		{
			var userChoice = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
			.Title($"[blue]No categories to {text}[red] please add categories[/][/]")
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
			.Title($"[blue]Which category do you want to {text}?[/]")
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