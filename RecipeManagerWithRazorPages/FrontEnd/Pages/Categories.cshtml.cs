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
        [TempData]
        public string CategoryInAlert { get; set; } = "";
        [TempData]
        public string NewCategoryInAlert { get; set; } = "";
        public bool OptionFlag { get; set; } = false;
        public string? CategoryToDelete;
        public string? CategoryToEdit;
        public string? NewCategoryTitle;

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

        public ActionResult OnPostEdit(string categoryToEdit,string newCategoryTitle)
        {
            TempData["CategoryInAlert"] = categoryToEdit;
            TempData["NewCategoryInAlert"] = newCategoryTitle;
            if (CategoriesRequests.Categories.Contains(newCategoryTitle))
            {
                TempData["Option"] = "editError";
            }
            else
            {
                TempData["Option"] = "edit";
                CategoriesRequests.UpdateCategoryAsync(categoryToEdit, newCategoryTitle).Wait();
            }

            return RedirectToPage("/Categories");
        }
    }
}
