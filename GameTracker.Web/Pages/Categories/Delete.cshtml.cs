using GameTracker.Models;
using GameTracker.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameTracker.Web.Pages.Categories;

public class DeleteModel : PageModel
{
    private readonly ICategoryRepository _categoryRepo;

    public Category? Category { get; set; }
    public string? ErrorMessage { get; set; }
    public bool CanDelete { get; set; }

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public DeleteModel(ICategoryRepository categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }

    public IActionResult OnGet()
    {
        Category = _categoryRepo.GetById(Id);
        if (Category == null) return NotFound();
        CanDelete = _categoryRepo.CanDelete(Id);
        return Page();
    }

    public IActionResult OnPost()
    {
        Category = _categoryRepo.GetById(Id);
        if (Category == null) return NotFound();

        try
        {
            _categoryRepo.Delete(Id);
            return RedirectToPage("/Categories/Index");
        }
        catch (InvalidOperationException ex)
        {
            ErrorMessage = ex.Message;
            return Page();
        }
    }
}
