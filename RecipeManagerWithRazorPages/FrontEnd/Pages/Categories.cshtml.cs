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
        [TempData]
        public string Option { get; set; } = "";
        public bool OptionFlag { get; set; } = false;
        public string? CategoryToDelete;
        [TempData]
        public string CategoryInAlert { get; set; } = "";

        public void OnGet()
        {
            CategoriesRequests.GetListOfCategoriesAsync().Wait();
        }

        public ActionResult OnPostDelete(string categoryToDelete)
        {
            TempData["Option"] = "delete";
            TempData["CategoryInAlert"] = categoryToDelete;
            CategoriesRequests.DeleteCategoryAsync(categoryToDelete).Wait();
            return RedirectToPage("/Categories");
        }
    }
}
