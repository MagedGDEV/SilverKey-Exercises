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
        public void OnGet(Guid recipeId)
        {
            RecipeId = recipeId;
            Recipe = RecipeRequests.Recipes[recipeId];
        }
    }
}
