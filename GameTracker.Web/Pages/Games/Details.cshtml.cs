using GameTracker.Models;
using GameTracker.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameTracker.Web.Pages.Games;

public class DetailsModel : PageModel
{
    private readonly IGameRepository _gameRepo;

    public Game? Game { get; set; }

    public DetailsModel(IGameRepository gameRepo)
    {
        _gameRepo = gameRepo;
    }

    public IActionResult OnGet(int id)
    {
        Game = _gameRepo.GetByIdWithRelations(id);
        if (Game == null)
            return NotFound();
        return Page();
    }
}
