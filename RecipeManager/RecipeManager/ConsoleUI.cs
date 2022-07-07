using Spectre.Console;
using System.Text;
public class ConsoleUI
{
	private bool _entryPoint;
	private RecipeManagement _manager;
	public ConsoleUI()
	{
		_entryPoint = true;
		_manager = new();
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
		_manager.Serialize();
		var goodByeMessage = new Markup("[red]GoodBye, it was nice helping you :smiling_face_with_smiling_eyes:[/]").Centered();
		AnsiConsole.Write(goodByeMessage);
	}

	private void WelcomeChoices()
	{
		if (_entryPoint)
        {
			AnsiConsole.Write("\n");
			_entryPoint = false;
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
		if (_entryPoint)
		{
			AnsiConsole.Write("\n");
			_entryPoint = false;
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
				ListRecipe();
				break;
			case "Add recipe":
				AddRecipe();
				break;
			case "Edit recipe":
				RecipesMenu("edit");
				break;
			case "Delete recipe":
				RecipesMenu("delete");
				break;
			case "Back":
				WelcomeChoices();
				break;
			default:
				ExitMessage();
				break;
		}
	}

	private void RecipesMenu(string text)
    {
		string[] choices = { "Back", "Exit" };
		if (_manager.Recipes.Count == 0)
        {
			var userChoice = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
			.Title($"[blue]No recipes to {text}[red] please add recipes[/][/]")
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
			Dictionary<string, Guid> recipes = new();
			foreach (KeyValuePair<Guid, Recipe> recipe in _manager.Recipes)
			{
				recipes.Add(recipe.Value.Title, recipe.Key);
			}
			var searchRecipes = recipes.Keys.ToList().Concat(choices);
			var userChoice = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
			.Title($"[blue]Which recipes do you which to {text}?[/]")
			.PageSize(5)
			.AddChoices(searchRecipes));
			switch (userChoice)
			{
				case "Back":
					RecipeChoices();
					break;
				case "Exit":
					ExitMessage();
					break;
				default:
					if (text == "edit")
                    {
						EditRecipe(recipes[userChoice]);
                    }
					else
                    {
						DeleteRecipe(recipes[userChoice], userChoice);
                    }
					break;
			}
		}
		
	}

	private void EditRecipe(Guid id)
    {
		var recipe = _manager.Recipes[id];
		string[] choices = { "Title", "Ingredients", "Instructions", "Categories", "Back", "Exit"};
		var userChoice = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
			.Title("[blue]Which item do you which to edit?[/]")
			.PageSize(5)
			.AddChoices(choices));
        switch (userChoice)
        {
            case "Title":
                //TODO: edit title of recipe
                EditTitle(recipe.Title, id);
                break;
            case "Ingredients":
                //TODO: edit ingredients of recipe
                break;
            case "Instructions":
                //TODO: edit instructions of the recipe 
                break;
            case "Categories":
                //TODO: edit categories of the recipe
                break;
            case "Back":
                RecipesMenu("edit");
                break;
            default:
                ExitMessage();
                break;
        }
    }

	private void EditTitle(string title, Guid id)
    {
		var updatedTitle = AnsiConsole.Prompt(
		new TextPrompt<string>($"[blue]Edit recipe [green]{title}[/] to:[/]")
		.PromptStyle("red")
		);
		_manager.Recipes[id].EditTitle(updatedTitle);
		AnsiConsole.Write(new Markup("[red]saved[/]"));
		Thread.Sleep(1000);
		AnsiConsole.Clear();
		EditRecipe(id);
	}

	private void EditListView (string listName, Guid id)
    {
		string[] choices = { "Edit", "Remove", "Add",  "Back", "Exit" };
		var userChoice = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
			.Title($"[blue]What would you like to do with recipe's {listName}?[/]")
			.PageSize(5)
			.AddChoices(choices));
		switch (userChoice)
        {
			case "Add":
				//TODO: add ingredient or instruction
				break;
			case "Edit":
				//TODO: edit ingredients or instructions
				break;
			case "Delete":
				//TODO: Delete ingredient or instruction
				DeleteList(id, listName);
				break;
			case "Back":
				EditRecipe(id);
				break;
			default:
				ExitMessage();
				break;
        }
	}

	private void DeleteList (Guid id, string listName)
    {
		var recipe = _manager.Recipes[id];
		string[] choices = {"Back", "Exit" };
		string [] deleteItems;
		if (listName == "Ingredients")
        {
			deleteItems = recipe.Ingredients.Concat(choices).ToArray();
		}
		else
        {
			deleteItems = recipe.Instructions.Concat(choices).ToArray();
		}

		var userChoice = AnsiConsole.Prompt(
		new SelectionPrompt<string>()
		.Title($"[blue]Which {listName} would you like to delete?[/]")
		.PageSize(5)
		.AddChoices(deleteItems));

		//TODO: delete item 
	}

	private void DeleteRecipe(Guid id, string recipeTitle)
    {
		_manager.DeleteRecipe(id);
		var deleteText = new Markup($"[red]{recipeTitle}[/][blue] is deleted[/]");
		AnsiConsole.Write(deleteText);
		Thread.Sleep(1000);
		AnsiConsole.Clear();
		ReDraw(true);
	}

	private List<string> RecipeQuestions(string listName)
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
		foreach (KeyValuePair<Guid, Recipe> recipe in _manager.Recipes)
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
		ArgumentNullException.ThrowIfNull(_manager.Categories);
		if (_manager.Categories.Count == 0)
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
			.AddChoices(_manager.Categories
			));
			_manager.AddRecipe(title, ingredients, instructions, categories);
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
		if (_entryPoint)
		{
			AnsiConsole.Write("\n");
			_entryPoint = false;
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
		bool added = _manager.AddCategory(category);
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
		ArgumentNullException.ThrowIfNull(_manager.Categories);
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
		if (_manager.Categories.Count == 0)
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
			var display = _manager.Categories.ToArray();
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
		_manager.DeleteCategory(category);
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
		_manager.EditCategory(category, updatedCategory);
		AnsiConsole.Write(new Markup("[red]saved[/]"));
		Thread.Sleep(1000);
		AnsiConsole.Clear();
		ReDraw(false);
	}

	private void ReDraw(bool menu)
    {
		_entryPoint = true;
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