using GameTracker.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameTracker.Web.Pages.Games;

public class IndexModel : PageModel
{
    private readonly IGameRepository _gameRepo;

    public List<GameTracker.Models.Game> Games { get; set; } = new();

    public IndexModel(IGameRepository gameRepo)
    {
        _gameRepo = gameRepo;
    }

    public void OnGet()
    {
        Games = _gameRepo.GetAll();
    }
}
