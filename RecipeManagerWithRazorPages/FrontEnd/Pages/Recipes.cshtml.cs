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
        public void OnGet()
        {
            RecipeRequests.GetRecipesImagesAsync().Wait();
            RecipeRequests.GetDictionaryOfRecipesAsync().Wait();

        }

        public string GetImagePath(string imageName)
        {
            Debug.Write(Path.Combine(Environment.CurrentDirectory, @"Images", imageName));
            return Path.Combine(Environment.CurrentDirectory, @"Images", imageName);
        }

    }
}
