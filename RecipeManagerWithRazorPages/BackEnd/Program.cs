var builder = WebApplication.CreateBuilder(args);
var recipes = new RecipesServices();
var categories = new CategoriesServices();
var app = builder.Build();
app.MapGet("/", () => "Hello World");
categories.Routing(app);
recipes.Routing(app);
app.Run();