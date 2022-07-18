var builder = WebApplication.CreateBuilder(args);
var categories = new CategoriesServices();
var app = builder.Build();

app.MapGet("/", () => "Hello World");
categories.Routing(app);
app.Run();