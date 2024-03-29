﻿var builder = WebApplication.CreateBuilder(args);
var recipes = new RecipesServices();
var categories = new CategoriesServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
categories.Routing(app);
recipes.Routing(app);
app.Run();