﻿using System;
using Spectre.Console;
public class ConsoleUI
{
	bool EntryPoint;
	public ConsoleUI()
	{
		EntryPoint = true;
		ConsoleTitle();
		WelcomeMessage();
		WelcomeChoices();
	}

	public void ConsoleTitle()
	{
		var titleText = new FigletText("Recipe Manager").Centered().Color(Color.Blue);
		var roundedPanel = new Panel(titleText).DoubleBorder();
		AnsiConsole.Write(roundedPanel);

	}

	public void WelcomeMessage()
	{	
		AnsiConsole.Write("\n");
		var welcomeText = new Markup("[bold yellow]Welcome to Recipe Manager [/]").Centered();
		AnsiConsole.Write(welcomeText);
		AnsiConsole.Write("\n");
	}

	public void WelcomeChoices()
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

				break;
			default:
				var goodByeMessage = new Markup("[red]GoodBye, it was nice helping you[/]").Centered();
				AnsiConsole.Write(goodByeMessage);
				break;
		}

	}

	public void RecipeChoices()
    {
		
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
				var goodByeMessage = new Markup("[red]GoodBye, it was nice helping you[/]").Centered();
				AnsiConsole.Write(goodByeMessage);
				break;
		}
	}
}