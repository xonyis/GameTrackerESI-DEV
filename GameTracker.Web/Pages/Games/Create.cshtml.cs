using GameTracker.Models;
using GameTracker.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameTracker.Web.Pages.Games;

public class CreateModel : PageModel
{
    private readonly IGameRepository _gameRepo;
    private readonly ICategoryRepository _categoryRepo;
    private readonly IStudioRepository _studioRepo;

    public List<SelectListItem> Categories { get; set; } = new(); // Id as Value, Name as Text
    public List<Studio> Studios { get; set; } = new();

    [BindProperty]
    public CreateInput Input { get; set; } = new();

    public class CreateInput
    {
        public string Title { get; set; } = string.Empty;
        public int? ReleaseYear { get; set; }
        public List<int> CategoryIds { get; set; } = new();
        public List<int> StudioIds { get; set; } = new();
    }

    public CreateModel(IGameRepository gameRepo, ICategoryRepository categoryRepo, IStudioRepository studioRepo)
    {
        _gameRepo = gameRepo;
        _categoryRepo = categoryRepo;
        _studioRepo = studioRepo;
    }

    public void OnGet()
    {
        Categories = _categoryRepo.GetAll()
            .Select(c => new SelectListItem(c.Name, c.Id.ToString()))
            .ToList();
        Studios = _studioRepo.GetAll();
    }

    public IActionResult OnPost()
    {
        if (string.IsNullOrWhiteSpace(Input.Title))
            ModelState.AddModelError("Input.Title", "Le titre est requis.");

        if (Input.CategoryIds == null || !Input.CategoryIds.Any())
            ModelState.AddModelError("Input.CategoryIds", "Sélectionnez au moins une catégorie.");

        if (!ModelState.IsValid)
        {
            OnGet();
            return Page();
        }

        var categories = _categoryRepo.GetAll().Where(c => Input.CategoryIds.Contains(c.Id)).ToList();
        var studios = Input.StudioIds.Count > 0
            ? _studioRepo.GetAll().Where(s => Input.StudioIds.Contains(s.Id)).ToList()
            : new List<Studio>();

        var game = new Game
        {
            Title = Input.Title.Trim(),
            ReleaseYear = Input.ReleaseYear,
            Categories = categories,
            Studios = studios
        };

        _gameRepo.Add(game);
        return RedirectToPage("/Games/Index");
    }
}
