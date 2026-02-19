using GameTracker.Models;
using GameTracker.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameTracker.Web.Pages.Categories;

public class CreateModel : PageModel
{
    private readonly ICategoryRepository _categoryRepo;

    [BindProperty]
    public CreateInput Input { get; set; } = new();

    public class CreateInput
    {
        public string Name { get; set; } = string.Empty;
    }

    public CreateModel(ICategoryRepository categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }

    public void OnGet() { }

    public IActionResult OnPost()
    {
        if (string.IsNullOrWhiteSpace(Input.Name))
        {
            ModelState.AddModelError("Input.Name", "Le nom est requis.");
        }

        if (!ModelState.IsValid) return Page();

        var category = new Category { Name = Input.Name.Trim() };
        _categoryRepo.Add(category);
        return RedirectToPage("/Categories/Index");
    }
}
