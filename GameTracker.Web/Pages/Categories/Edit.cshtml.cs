using GameTracker.Models;
using GameTracker.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameTracker.Web.Pages.Categories;

public class EditModel : PageModel
{
    private readonly ICategoryRepository _categoryRepo;

    [BindProperty]
    public EditInput Input { get; set; } = new();

    public class EditInput
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public EditModel(ICategoryRepository categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }

    public IActionResult OnGet(int id)
    {
        var category = _categoryRepo.GetById(id);
        if (category == null) return NotFound();

        Input = new EditInput { Id = category.Id, Name = category.Name };
        return Page();
    }

    public IActionResult OnPost()
    {
        var category = _categoryRepo.GetById(Input.Id);
        if (category == null) return NotFound();

        if (string.IsNullOrWhiteSpace(Input.Name))
        {
            ModelState.AddModelError("Input.Name", "Le nom est requis.");
        }

        if (!ModelState.IsValid) return Page();

        var toUpdate = new Category { Id = Input.Id, Name = Input.Name.Trim() };
        _categoryRepo.Update(Input.Id, toUpdate);
        return RedirectToPage("/Categories/Index");
    }
}
