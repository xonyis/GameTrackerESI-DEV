using GameTracker.Models;
using GameTracker.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameTracker.Web.Pages.Studios;

public class CreateModel : PageModel
{
    private readonly IStudioRepository _studioRepo;

    [BindProperty]
    public CreateInput Input { get; set; } = new();

    public class CreateInput
    {
        public string Name { get; set; } = string.Empty;
    }

    public CreateModel(IStudioRepository studioRepo)
    {
        _studioRepo = studioRepo;
    }

    public void OnGet() { }

    public IActionResult OnPost()
    {
        if (string.IsNullOrWhiteSpace(Input.Name))
        {
            ModelState.AddModelError("Input.Name", "Le nom est requis.");
        }

        if (!ModelState.IsValid) return Page();

        var studio = new Studio { Name = Input.Name.Trim() };
        _studioRepo.Add(studio);
        return RedirectToPage("/Studios/Index");
    }
}
