using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages
{
    public class CategoriesModel : PageModel
    {
        public string? CategoryToDelete;
        public void OnGet()
        {
            CategoriesRequests.GetListOfCategoriesAsync().Wait();
        }

        public IActionResult OnPostDelete(string categoryToDelete)
        {
            CategoriesRequests.DeleteCategoryAsync(categoryToDelete!).Wait();
            return RedirectToPage("/Categories");
        }
    }
}
