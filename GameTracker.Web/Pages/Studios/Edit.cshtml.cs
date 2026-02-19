using GameTracker.Models;
using GameTracker.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameTracker.Web.Pages.Studios;

public class EditModel : PageModel
{
    private readonly IStudioRepository _studioRepo;

    [BindProperty]
    public EditInput Input { get; set; } = new();

    public class EditInput
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public EditModel(IStudioRepository studioRepo)
    {
        _studioRepo = studioRepo;
    }

    public IActionResult OnGet(int id)
    {
        var studio = _studioRepo.GetById(id);
        if (studio == null) return NotFound();

        Input = new EditInput { Id = studio.Id, Name = studio.Name };
        return Page();
    }

    public IActionResult OnPost()
    {
        var studio = _studioRepo.GetById(Input.Id);
        if (studio == null) return NotFound();

        if (string.IsNullOrWhiteSpace(Input.Name))
        {
            ModelState.AddModelError("Input.Name", "Le nom est requis.");
        }

        if (!ModelState.IsValid) return Page();

        var toUpdate = new Studio { Id = Input.Id, Name = Input.Name.Trim() };
        _studioRepo.Update(Input.Id, toUpdate);
        return RedirectToPage("/Studios/Index");
    }
}
