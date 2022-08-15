using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages
{
    public class RecipesModel : PageModel
    {
        public Guid? RecipeId;
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
    }
}
