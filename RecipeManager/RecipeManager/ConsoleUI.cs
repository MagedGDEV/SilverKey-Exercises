using System;
using Spectre.Console;
public class ConsoleUI
{
	public ConsoleUI()
	{
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
		var header = new Markup("What would you are looking for today ?");
		AnsiConsole.Write(header);
	}
}