using Spectre.Console;
using System.Text;
public class ConsoleUI
{
	private bool _entryPoint;
	private RecipeManagement _manager;
	private int _sleepTime;
	public ConsoleUI()
	{
		_entryPoint = true;
		_manager = new();
		_sleepTime = 2000;
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
                EditTitle(recipe.Title, id);
                break;
            case "Ingredients":
				EditListView("ingredients", id);
                break;
            case "Instructions":
				EditListView("instructions", id);
                break;
            case "Categories":
				EditCategoryInRecipe(id);
				break;
            case "Back":
                RecipesMenu("edit");
                break;
            default:
                ExitMessage();
                break;
        }
    }

	private void EditCategoryInRecipe(Guid id)
    {
		string[] choices = { "Add", "Remove", "Back", "Exit" };
		var userChoice = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
			.Title("[blue]What would you like to do with recipe's categories?[/]")
			.PageSize(5)
			.AddChoices(choices));
		switch (userChoice)
        {
			case "Add":
				AddCategoryToRecipe(id);
				break;
			case "Remove":
				RemoveCategory(id);
				break;
			case "Back":
				EditRecipe(id);
				break;
			default:
				ExitMessage();
				break;
        }
	}

	private void AddCategoryToRecipe(Guid id)
    {
		ArgumentNullException.ThrowIfNull(_manager.Categories);
		List<string> categoriesAvailable = new();
		categoriesAvailable = _manager.Categories.Except(_manager.Recipes[id].Categories).ToList();
		if (categoriesAvailable.Count == 0)
        {
			AnsiConsole.Write("\n");
			AnsiConsole.Write(new Markup("[blue]This recipe already has all the available categories [red]Add more categories first[/][/]"));
			Thread.Sleep(_sleepTime);
			AnsiConsole.Clear();
			ConsoleTitle();
			WelcomeMessage();
			EditCategoryInRecipe(id);
		}
        else
        {
            List<string> categories = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
            .Title("[blue]Which category to you which to add to the recipe?[/]")
            .PageSize(5)
            .NotRequired()
            .InstructionsText(
                "[grey](Press [blue]<space>[/] to toggle a category, " +
                "[green]<enter>[/] to accept)[/]")
            .AddChoices(categoriesAvailable)
            );
            if (categories.Count == 0)
            {

                AnsiConsole.Write(new Markup("[red]No category selected[/]"));
                Thread.Sleep(_sleepTime);
                AnsiConsole.Clear();
                ConsoleTitle();
                WelcomeMessage();
                EditCategoryInRecipe(id);
            }
            else
            {
                for (int i = 0; i < categories.Count; i++)
                {
                    _manager.Recipes[id].AddCategory(categories[i]);

                }
                AnsiConsole.Clear();
                ConsoleTitle();
                WelcomeMessage();
                AnsiConsole.Write("\n");
                AnsiConsole.Write(new Markup("[red]Saved[/]"));
                Thread.Sleep(_sleepTime);
                AnsiConsole.Clear();
                ConsoleTitle();
                WelcomeMessage();
                EditCategoryInRecipe(id);
            }

        }
    }

	private void RemoveCategory(Guid id)
    {
		if (_manager.Recipes[id].Categories.Count == 1)
		{
			AnsiConsole.Write(new Markup("[blue]This recipe can't remove categories since it has only one category [red]Please add another category first[/][/]"));
			Thread.Sleep(_sleepTime);
			AnsiConsole.Clear();
			ConsoleTitle();
			WelcomeMessage();
			EditCategoryInRecipe(id);
		}
		else
		{
			List<string> userChoice = AnsiConsole.Prompt(
			new MultiSelectionPrompt<string>()
			.Title($"[blue]Which category would you like to remove?[/]")
			.PageSize(5)
			.NotRequired()
			.InstructionsText(
					"[grey](Press [blue]<space>[/] to toggle a category, " +
					"[green]<enter>[/] to accept)[/]")
			.AddChoices(_manager.Recipes[id].Categories));

			if (userChoice.Count == 0)
            {
				AnsiConsole.Write(new Markup("[red]No category selected to be removed[/]"));
				Thread.Sleep(_sleepTime);
				AnsiConsole.Clear();
				ConsoleTitle();
				WelcomeMessage();
				EditCategoryInRecipe(id);
			}
			else if (userChoice.Count == _manager.Recipes[id].Categories.Count)
			{
				AnsiConsole.Write(new Markup("[blue]A recipe must have atleast one category [red]Please add more categories first[/][/]"));
				Thread.Sleep(_sleepTime);
				AnsiConsole.Clear();
				ConsoleTitle();
				WelcomeMessage();
				EditCategoryInRecipe(id);
			}
			else
			{
				AnsiConsole.Write(new Markup("[red]Categories selected are removed[/]"));
				for (int i = 0; i < userChoice.Count; i++)
                {
					_manager.Recipes[id].DeleteCategory(userChoice[i]);
				}
				Thread.Sleep(_sleepTime);
				AnsiConsole.Clear();
				ConsoleTitle();
				WelcomeMessage();
				EditCategoryInRecipe(id);
			}
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
		Thread.Sleep(_sleepTime);
		AnsiConsole.Clear();
		EditRecipe(id);
	}

	private void EditListView (string listName, Guid id)
    {
		string[] choices = { "Add", "Edit", "Delete", "Back", "Exit" };
		var userChoice = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
			.Title($"[blue]What would you like to do with recipe's {listName}?[/]")
			.PageSize(5)
			.AddChoices(choices));
		switch (userChoice)
        {
			case "Add":
				AddToList(listName, id);
				break;
			case "Edit":
				EditList(listName, id);
				break;
			case "Delete":
				DeleteList(listName, id);
				break;
			case "Back":
				EditRecipe(id);
				break;
			default:
				ExitMessage();
				break;
        }
	}

	private void AddToList (string listName, Guid id)
    {
		var itemToAdd = AnsiConsole.Prompt(
				new TextPrompt<string>($"[blue]The new {listName.Remove(listName.Length-1)} is:[/]")
				.PromptStyle("red")
				);
		var index = AnsiConsole.Prompt(
			new TextPrompt<string>("[blue]Which position do you want to place it:[/]")
			.PromptStyle("red")
			);
		if (listName == "ingredients")
		{
			_manager.Recipes[id].AddIngredient(itemToAdd, Int16.Parse(index));
		}
		else
		{
			_manager.Recipes[id].AddInstruction(itemToAdd, Int16.Parse(index));
		}
		AnsiConsole.Clear();
		ConsoleTitle();
		WelcomeMessage();
		AnsiConsole.Write("\n");
		AnsiConsole.Write(new Markup("[red]Saved[/]"));
		Thread.Sleep(_sleepTime);
		AnsiConsole.Clear();
		ConsoleTitle();
		WelcomeMessage();
		EditListView(listName, id);

	}

	private void EditList (string listName, Guid id)
    {
		var recipe = _manager.Recipes[id];
		List<string> editItems = new();
		string[] choices = { "Back", "Exit"};
		if (listName == "ingredients")
		{
			editItems = recipe.Ingredients.Concat(choices).ToList();
		}
		else
		{
			editItems = recipe.Instructions.Concat(choices).ToList();
		}
		var userChoice = AnsiConsole.Prompt(
		new SelectionPrompt<string>()
		.Title($"[blue]Which {listName} would you like to edit?[/]")
		.PageSize(5)
		.AddChoices(editItems));
		switch (userChoice)
        {
			case "Back":
				EditListView(listName, id);
				break;
			case "Exit":
				ExitMessage();
				break;
			default:
				var updated = AnsiConsole.Prompt(
				new TextPrompt<string>($"[blue]Edit category [green]{userChoice}[/] to:[/]")
				.PromptStyle("red")
				);
				AnsiConsole.Clear();
				ConsoleTitle();
				WelcomeMessage();
				AnsiConsole.Write("\n");
				AnsiConsole.Write(new Markup("[red]Saved[/]"));
				if (listName == "ingredients")
				{
					_manager.Recipes[id].EditIngredients(userChoice, updated);
				}
				else
				{
					_manager.Recipes[id].EditInstruction(userChoice, updated);
				}
				Thread.Sleep(_sleepTime);
				AnsiConsole.Clear();
				ConsoleTitle();
				WelcomeMessage();
				EditListView(listName, id);
				break;
        }
	}

	private void DeleteList (string listName, Guid id)
    {
		var recipe = _manager.Recipes[id];
		List<string> deleteItems = new();
		if (listName == "ingredients")
        {
			deleteItems = recipe.Ingredients.ToList();
		}
		else
        {
			deleteItems = recipe.Instructions.ToList();
		}
		List<string> userChoice = AnsiConsole.Prompt(
		new MultiSelectionPrompt<string>()
		.Title($"[blue]Which {listName} would you like to delete?[/]")
		.PageSize(5)
		.NotRequired()
		.InstructionsText(
				$"[grey](Press [blue]<space>[/] to toggle a {listName}, " +
				"[green]<enter>[/] to accept)[/]")
		.AddChoices(deleteItems));
		if (userChoice.Count == 0)
        {
			AnsiConsole.Write(new Markup("[red]No items selected[/]"));
        }
		else
        {
			for (int i = 0; i < userChoice.Count; i++)
            {
				if (listName == "ingredients")
                {
					_manager.Recipes[id].DeleteIngredient(userChoice[i]);
				}
				else
                {
					_manager.Recipes[id].DeleteInstructions(userChoice[i]);
				}
            }
			AnsiConsole.Write(new Markup("[red]Items deleted[/]"));
		}
		Thread.Sleep(_sleepTime);
		AnsiConsole.Clear();
		ConsoleTitle();
		WelcomeMessage();
		AnsiConsole.Write("\n");
		EditListView(listName, id);
	}

	private void DeleteRecipe(Guid id, string recipeTitle)
    {
		_manager.DeleteRecipe(id);
		var deleteText = new Markup($"[red]{recipeTitle}[/][blue] is deleted[/]");
		AnsiConsole.Write(deleteText);
		Thread.Sleep(_sleepTime);
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
			.PageSize(5)
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
			Thread.Sleep(_sleepTime);
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
		Thread.Sleep(_sleepTime);
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
		Thread.Sleep(_sleepTime);
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
		Thread.Sleep(_sleepTime);
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