using GameTracker.Models;
using GameTracker.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameTracker.Web.Pages.Users;

public class DetailsModel : PageModel
{
    private readonly IUserRepository _userRepo;
    private readonly IGameRepository _gameRepo;
    private readonly ICategoryRepository _categoryRepo;

    public User? CurrentUser { get; set; }
    public List<Game> GamesPlayed { get; set; } = new();
    public List<SelectListItem> Categories { get; set; } = new();
    public int? CategoryFilter { get; set; }

    public DetailsModel(IUserRepository userRepo, IGameRepository gameRepo, ICategoryRepository categoryRepo)
    {
        _userRepo = userRepo;
        _gameRepo = gameRepo;
        _categoryRepo = categoryRepo;
    }

    public IActionResult OnGet(int id, int? categoryId)
    {
        CurrentUser = _userRepo.GetById(id);
        if (CurrentUser == null)
            return NotFound();

        CategoryFilter = categoryId;
        GamesPlayed = categoryId.HasValue
            ? _gameRepo.GetByUserIdAndCategory(id, categoryId.Value)
            : _gameRepo.GetByUserId(id);

        Categories = _categoryRepo.GetAll()
            .Select(c => new SelectListItem(c.Name, c.Id.ToString(), categoryId.HasValue && c.Id == categoryId.Value))
            .ToList();

        return Page();
    }
}
