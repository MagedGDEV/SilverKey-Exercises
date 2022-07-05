using System;
using Spectre.Console;
public class ConsoleUI
{
	public ConsoleUI()
	{
		ConsoleTitle();
	}

	public void ConsoleTitle()
    {
		
		AnsiConsole.Write(
		new Panel(
		new FigletText("Recipe Manager")
	   .Centered()
	   
	   .Color(Color.Blue))
		.HeavyBorder()
		.RoundedBorder());
	}
}