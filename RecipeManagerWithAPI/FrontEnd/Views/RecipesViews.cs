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
        RecipeRequests.GetDictionaryOfRecipesAsync().Wait();
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
                AddRecipe();
                break;
            case "Edit recipe":
                RecipesMenu("edit");
                break;
            case "Delete recipe":
                RecipesMenu("delete");
                break;
            case "Back":
                ConsoleViews.IntialChoices();
                break;
            default:
                ConsoleViews.ExitMessage();
                break;
        }
    }

    static private void AddRecipe()
    {
        CategoriesRequests.GetListOfCategoriesAsync().Wait();
        if (CategoriesRequests.Categories.Count == 0)
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
                    RecipeOptions();
                    break;
                default:
                    ConsoleViews.ExitMessage();
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
            .AddChoices(CategoriesRequests.Categories
            ));
            ConsoleViews.ExitingView();
            AnsiConsole.Write(new Markup("[red]Saved[/]"));
            var recipe = new RecipeModel(title, ingredients, instructions, categories);
            RecipeRequests.AddRecipeAsync(recipe).Wait();
            Thread.Sleep(ConsoleViews.SleepTime);
            ConsoleViews.ExitingView();
            RecipeOptions();
        }
    }

    static private List<string> RecipeQuestions(string listName)
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

    static private void RecipesMenu(string text)
    {
        string[] choices = { "Back", "Exit" };
        if (RecipeRequests.Recipes.Count == 0)
        {
            var userChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title($"[blue]No recipes to {text}[red] please add recipes[/][/]")
            .PageSize(5)
            .AddChoices(choices));
            switch (userChoice)
            {
                case "Back":
                    RecipeOptions();
                    break;
                default:
                    ConsoleViews.ExitMessage();
                    break;
            }
        }
        else
        {
            Dictionary<string, Guid> recipes = new();
            foreach (KeyValuePair<Guid, RecipeModel> recipe in RecipeRequests.Recipes)
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
                    RecipeOptions();
                    break;
                case "Exit":
                    ConsoleViews.ExitMessage();
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

    static private void DeleteRecipe(Guid id, string recipeTitle)
    {
        
        var deleteText = new Markup($"[red]{recipeTitle}[/][blue] is deleted[/]");
        RecipeRequests.DeleteRecipesAsync(id).Wait();
        AnsiConsole.Write(deleteText);
        Thread.Sleep(ConsoleViews.SleepTime);
        ConsoleViews.ExitingView();
        RecipeOptions();
    }

    static private void EditRecipe(Guid id)
    {
        var recipe = RecipeRequests.Recipes[id];
        string[] choices = { "Title", "Ingredients", "Instructions", "Categories", "Back", "Exit" };
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
                ConsoleViews.ExitMessage();
                break;
        }
    }

    static private void EditTitle(string title, Guid id)
    {
        var updatedTitle = AnsiConsole.Prompt(
        new TextPrompt<string>($"[blue]Edit recipe [green]{title}[/] to:[/]")
        .PromptStyle("red")
        );
        AnsiConsole.Write(new Markup("[red]saved[/]"));
        //TODO: HTTP request to edit title
        Thread.Sleep(ConsoleViews.SleepTime);
        ConsoleViews.ExitingView();
        EditRecipe(id);
    }

    static private void EditListView(string listName, Guid id)
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
                ConsoleViews.ExitMessage();
                break;
        }
    }

    static private void AddToList(string listName, Guid id)
    {
        var itemToAdd = AnsiConsole.Prompt(
                new TextPrompt<string>($"[blue]The new {listName.Remove(listName.Length - 1)} is:[/]")
                .PromptStyle("red")
                );
        var index = AnsiConsole.Prompt(
            new TextPrompt<string>("[blue]Which position do you want to place it:[/]")
            .PromptStyle("red")
            );
        ConsoleViews.ExitingView();
        AnsiConsole.Write(new Markup("[red]Saved[/]"));
        if (listName == "ingredients")
        {
            var ingredient = new AddListModel(itemToAdd, Int16.Parse(index));
            RecipeRequests.AddRecipeIngredientsAsync(ingredient, id).Wait();
        }
        else
        {
            var instruction = new AddListModel(itemToAdd, Int16.Parse(index));
            RecipeRequests.AddRecipeInstructionsAsync(instruction, id).Wait();
        }
        Thread.Sleep(ConsoleViews.SleepTime);
        ConsoleViews.ExitingView();
        EditListView(listName, id);
    }

    static private void EditList(string listName, Guid id)
    {
        var recipe = RecipeRequests.Recipes[id];
        List<string> editItems = new();
        string[] choices = { "Back", "Exit" };
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
                ConsoleViews.ExitMessage();
                break;
            default:
                var updated = AnsiConsole.Prompt(
                new TextPrompt<string>($"[blue]Edit category [green]{userChoice}[/] to:[/]")
                .PromptStyle("red")
                );
                ConsoleViews.ExitingView();
                AnsiConsole.Write(new Markup("[red]Saved[/]"));
                if (listName == "ingredients")
                {
                    RecipeRequests.UpdateRecipeIngredientsAsync(userChoice, updated, id).Wait();
                }
                else
                {
                    //TODO: HTTP REQUEST TO EDIT LIST IN INSTRUCTIONS
                }
                Thread.Sleep(ConsoleViews.SleepTime);
                ConsoleViews.ExitingView();
                EditListView(listName, id);
                break;
        }
    }

    static private void DeleteList(string listName, Guid id)
    {
        var recipe = RecipeRequests.Recipes[id];
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
            AnsiConsole.Write(new Markup("[red]Items deleted[/]"));
            for (int i = 0; i < userChoice.Count; i++)
            {
                if (listName == "ingredients")
                {
                    RecipeRequests.DeleteRecipeIngredientsAsync(userChoice, id).Wait();
                }
                else
                {
                    RecipeRequests.DeleteRecipeInstructionsAsync(userChoice, id).Wait();
                }
            }
        }
        Thread.Sleep(ConsoleViews.SleepTime);
        ConsoleViews.ExitingView();
        EditListView(listName, id);
    }

    static private void EditCategoryInRecipe(Guid id)
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
                ConsoleViews.ExitMessage();
                break;
        }
    }

    static private void AddCategoryToRecipe(Guid id)
    {
        CategoriesRequests.GetListOfCategoriesAsync().Wait();
        List<string> categoriesAvailable = new();
        categoriesAvailable = CategoriesRequests.Categories.Except(RecipeRequests.Recipes[id].Categories).ToList();
        if (categoriesAvailable.Count == 0)
        {
            AnsiConsole.Write("\n");
            AnsiConsole.Write(new Markup("[blue]This recipe already has all the available categories [red]Add more categories first[/][/]"));
            Thread.Sleep(ConsoleViews.SleepTime);
            ConsoleViews.ExitingView();
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
                Thread.Sleep(ConsoleViews.SleepTime);
                AnsiConsole.Clear();
                ConsoleViews.ExitingView();
                EditCategoryInRecipe(id);
            }
            else
            {
                ConsoleViews.ExitingView();
                AnsiConsole.Write(new Markup("[red]Saved[/]"));
                RecipeRequests.AddRecipeCategoriesAsync(categories, id).Wait();
                Thread.Sleep(ConsoleViews.SleepTime);
                ConsoleViews.ExitingView();
                EditCategoryInRecipe(id);
            }

        }
    }

    static private void RemoveCategory(Guid id)
    {
        if (RecipeRequests.Recipes[id].Categories.Count == 1)
        {
            AnsiConsole.Write(new Markup("[blue]This recipe can't remove categories since it has only one category [red]Please add another category first[/][/]"));
            Thread.Sleep(ConsoleViews.SleepTime);
            ConsoleViews.ExitingView();
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
            .AddChoices(RecipeRequests.Recipes[id].Categories));

            if (userChoice.Count == 0)
            {
                AnsiConsole.Write(new Markup("[red]No category selected to be removed[/]"));
                Thread.Sleep(ConsoleViews.SleepTime);
                ConsoleViews.ExitingView();
                EditCategoryInRecipe(id);
            }
            else if (userChoice.Count == RecipeRequests.Recipes[id].Categories.Count)
            {
                AnsiConsole.Write(new Markup("[blue]A recipe must have atleast one category [red]Please add more categories first[/][/]"));
                Thread.Sleep(ConsoleViews.SleepTime);
                ConsoleViews.ExitingView();
                EditCategoryInRecipe(id);
            }
            else
            {
                AnsiConsole.Write(new Markup("[red]Categories selected are removed[/]"));
                RecipeRequests.DeleteRecipeCategoriesAsync(userChoice, id).Wait();
                Thread.Sleep(ConsoleViews.SleepTime);
                ConsoleViews.ExitingView();
                EditCategoryInRecipe(id);
            }
        }
    }
}


