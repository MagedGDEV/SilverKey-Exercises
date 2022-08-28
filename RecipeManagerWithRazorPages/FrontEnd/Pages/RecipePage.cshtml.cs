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
        [TempData]
        public string RecipeRequest { get; set; } = "";
        [TempData]
        public string RecipeAlert { get; set; } = "";
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
            var allTitles = RecipeRequests.Recipes.Select(recipe => recipe.Value.Title).ToList();
            TempData["RecipeAlert"] = recipeTitle;
            if (allTitles.Contains(recipeTitle))
            {
                TempData["RecipeRequest"] = "editFalse";
            }
            else
            {
                TempData["RecipeRequest"] = "editTrue";
                RecipeRequests.GetDictionaryOfRecipesAsync().Wait();
                var newRecipe = new RecipeModel(recipeTitle, formIngredients.ToList(), formInstructions.ToList(), formCategories.ToList());
                newRecipe.ImageName = RecipeRequests.Recipes[recipeId].ImageName;
                RecipeRequests.UpdateRecipeAsync(newRecipe, recipeId).Wait();
                RecipeRequests.GetDictionaryOfRecipesAsync().Wait();
                RecipeRequests.GetRecipesImagesAsync().Wait();
            }
            return RedirectToPage("/RecipePage", new {recipeId});
        }

        public ActionResult OnPostDelete (Guid recipeId)
        {
            TempData["RecipeAlert"] = RecipeRequests.Recipes[recipeId].Title;
            TempData["RecipeRequest"] = "delete";
            RecipeRequests.DeleteRecipesAsync(recipeId).Wait();
            return RedirectToPage("/Recipes");
        }
    }
}
