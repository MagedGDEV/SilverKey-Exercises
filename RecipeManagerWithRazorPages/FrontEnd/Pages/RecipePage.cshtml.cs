using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages
{
    public class RecipePageModel : PageModel
    {
        public Guid RecipeId;
        public RecipeModel? Recipe;
        public string? RecipeTitle;
        public IFormFile? RecipeImage { get; set; }
        public void OnGet(Guid recipeId)
        {
            RecipeId = recipeId;
            Recipe = RecipeRequests.Recipes[recipeId];
        }

        public ActionResult OnPostEdit (string recipeTitle, IFormFile recipeImage, Guid recipeId)
        {
            if (recipeImage != null)
            {
                RecipeRequests.UpdateRecipeImageAsync(recipeImage!, recipeId).Wait();
            }
            string[] formIngredients = Request.Form["ingredient"];
            string[] formInstructions = Request.Form["instruction"];
            string[] formCategories = Request.Form["CategorySuccess"];
            RecipeRequests.GetRecipesImagesAsync().Wait();
            return RedirectToPage("/RecipePage", new {recipeId});
        }
    }
}
