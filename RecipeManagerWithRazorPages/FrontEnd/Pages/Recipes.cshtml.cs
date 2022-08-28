using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO.Compression;
namespace FrontEnd.Pages
{
    public class RecipesModel : PageModel
    {
        [TempData]
        public string Added { get; set; } = "";
        [TempData]
        public string Recipe { get; set; } = "";
        public Guid? RecipeId;
        public string? RecipeTitle;
        public IFormFile? RecipeImage { get; set; }

        public void OnGet()
        {
            RecipeRequests.GetRecipesImagesAsync().Wait();
            RecipeRequests.GetDictionaryOfRecipesAsync().Wait();
            CategoriesRequests.GetListOfCategoriesAsync().Wait();
        }

        public ActionResult OnPostRecipePage(Guid recipeId)
        {
            return RedirectToPage("/RecipePage", new {recipeId});
        }

        public ActionResult OnPostRecipe(string recipeTitle, IFormFile recipeImage)
        {
            string[] formIngredients = Request.Form["ingredient"];
            string[] formInstructions = Request.Form["instruction"];
            string[] formCategories = Request.Form["CategorySuccess"];
            var allTitles = RecipeRequests.Recipes.Select(recipe => recipe.Value.Title).ToList();
            TempData["Recipe"] = recipeTitle;
            if (allTitles.Contains(recipeTitle))
            {
                TempData["Added"] = "false";
            }
            else
            {
                TempData["Added"] = "true";
                var newRecipe = new RecipeModel(recipeTitle, formIngredients.ToList(), formInstructions.ToList(), formCategories.ToList());
                RecipeRequests.AddRecipeWithImageAsync(newRecipe, recipeImage).Wait();
            }
            return RedirectToPage("/Recipes");
        }
    }
}
