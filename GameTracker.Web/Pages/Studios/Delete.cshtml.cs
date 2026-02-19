using GameTracker.Models;
using GameTracker.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameTracker.Web.Pages.Studios;

public class DeleteModel : PageModel
{
    private readonly IStudioRepository _studioRepo;

    public Studio? Studio { get; set; }

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public DeleteModel(IStudioRepository studioRepo)
    {
        _studioRepo = studioRepo;
    }

    public IActionResult OnGet()
    {
        Studio = _studioRepo.GetById(Id);
        if (Studio == null) return NotFound();
        return Page();
    }

    public IActionResult OnPost()
    {
        var studio = _studioRepo.GetById(Id);
        if (studio == null) return NotFound();

        _studioRepo.Delete(Id);
        return RedirectToPage("/Studios/Index");
    }
}
