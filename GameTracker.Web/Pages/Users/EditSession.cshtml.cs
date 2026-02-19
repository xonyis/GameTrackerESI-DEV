using GameTracker.Models;
using GameTracker.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameTracker.Web.Pages.Users;

public class EditSessionModel : PageModel
{
    private readonly IUserRepository _userRepo;
    private readonly IPlaySessionRepository _sessionRepo;

    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string GameTitle { get; set; } = string.Empty;

    [BindProperty]
    public EditSessionInput Input { get; set; } = new();

    public class EditSessionInput
    {
        public int SessionId { get; set; }
        public DateTime Date { get; set; }
        public int HoursPlayed { get; set; }
    }

    public EditSessionModel(IUserRepository userRepo, IPlaySessionRepository sessionRepo)
    {
        _userRepo = userRepo;
        _sessionRepo = sessionRepo;
    }

    public IActionResult OnGet(int userId, int gameId)
    {
        var user = _userRepo.GetById(userId);
        if (user == null) return NotFound();

        var session = _sessionRepo.GetByUserAndGame(userId, gameId);
        if (session == null) return NotFound();

        UserId = userId;
        Username = user.Username;
        GameTitle = session.Game?.Title ?? "Jeu";
        Input = new EditSessionInput
        {
            SessionId = session.Id,
            Date = session.Date,
            HoursPlayed = session.HoursPlayed
        };

        return Page();
    }

    public IActionResult OnPost()
    {
        var session = _sessionRepo.GetById(Input.SessionId);
        if (session == null) return NotFound();

        UserId = session.UserId;
        Username = session.User?.Username ?? "";
        GameTitle = session.Game?.Title ?? "Jeu";

        var toUpdate = new PlaySession
        {
            Id = Input.SessionId,
            UserId = session.UserId,
            GameId = session.GameId,
            Date = Input.Date,
            HoursPlayed = Input.HoursPlayed
        };
        _sessionRepo.Update(Input.SessionId, toUpdate);
        return RedirectToPage("/Users/Details", new { id = session.UserId });
    }
}
