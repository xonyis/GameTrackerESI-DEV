using GameTracker.Models;
using GameTracker.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameTracker.Web.Pages.Games;

public class DeleteModel : PageModel
{
    private readonly IGameRepository _gameRepo;

    public Game? Game { get; set; }

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public DeleteModel(IGameRepository gameRepo)
    {
        _gameRepo = gameRepo;
    }

    public IActionResult OnGet()
    {
        Game = _gameRepo.GetByIdWithRelations(Id);
        if (Game == null) return NotFound();
        return Page();
    }

    public IActionResult OnPost()
    {
        var game = _gameRepo.GetByIdWithRelations(Id);
        if (game == null) return NotFound();

        _gameRepo.Delete(Id);
        return RedirectToPage("/Games/Index");
    }
}
