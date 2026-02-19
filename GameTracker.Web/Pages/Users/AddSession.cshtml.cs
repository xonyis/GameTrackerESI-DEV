using GameTracker.Models;
using GameTracker.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameTracker.Web.Pages.Users;

public class AddSessionModel : PageModel
{
    private readonly IUserRepository _userRepo;
    private readonly IGameRepository _gameRepo;
    private readonly IPlaySessionRepository _sessionRepo;

    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public List<SelectListItem> Games { get; set; } = new();
    public int GamesCount { get; set; }
    public int GamesAvailableCount { get; set; }

    [BindProperty]
    public AddSessionInput Input { get; set; } = new();

    public class AddSessionInput
    {
        public int UserId { get; set; }
        public int GameId { get; set; }
        public DateTime Date { get; set; }
        public int HoursPlayed { get; set; }
    }

    public AddSessionModel(IUserRepository userRepo, IGameRepository gameRepo, IPlaySessionRepository sessionRepo)
    {
        _userRepo = userRepo;
        _gameRepo = gameRepo;
        _sessionRepo = sessionRepo;
    }

    public IActionResult OnGet(int id)
    {
        var user = _userRepo.GetById(id);
        if (user == null) return NotFound();

        UserId = user.Id;
        Username = user.Username;
        Input.UserId = user.Id;
        Input.Date = DateTime.Today;

        var allGames = _gameRepo.GetAll();
        var gamesPlayed = _gameRepo.GetByUserId(id).Select(g => g.Id).ToHashSet();
        Games = allGames
            .Where(g => !gamesPlayed.Contains(g.Id))
            .Select(g => new SelectListItem(g.Title, g.Id.ToString()))
            .ToList();

        GamesCount = allGames.Count;
        GamesAvailableCount = Games.Count;
        return Page();
    }

    public IActionResult OnPost()
    {
        var user = _userRepo.GetById(Input.UserId);
        if (user == null) return NotFound();

        UserId = user.Id;
        Username = user.Username;
        Games = _gameRepo.GetAll().Select(g => new SelectListItem(g.Title, g.Id.ToString())).ToList();

        var existing = _sessionRepo.GetByUserAndGame(Input.UserId, Input.GameId);
        if (existing != null)
        {
            ModelState.AddModelError("Input.GameId", "Une session existe déjà pour ce jeu. Utilisez « Modifier » pour mettre à jour les heures.");
            return Page();
        }

        var game = _gameRepo.GetByIdWithRelations(Input.GameId);
        if (game == null)
        {
            ModelState.AddModelError("Input.GameId", "Jeu invalide.");
            return Page();
        }

        var session = new PlaySession
        {
            UserId = Input.UserId,
            GameId = Input.GameId,
            Date = Input.Date,
            HoursPlayed = Input.HoursPlayed
        };
        _sessionRepo.Add(session);
        return RedirectToPage("/Users/Details", new { id = Input.UserId });
    }
}
