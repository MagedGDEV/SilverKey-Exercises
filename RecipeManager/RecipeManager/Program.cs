// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var recipie = new Recipie("eggs", new List<string>() { "hot", "old" }, new List<string>() { "cold"}, new List<string>() { "old"});
recipie.printRecipe();
recipie.DeleteData("hot", "Ingredients");
recipie.printRecipe();
recipie.AddData("hot", "Ingredients");
recipie.printRecipe();