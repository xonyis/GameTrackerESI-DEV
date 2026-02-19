using GameTracker.Models;
using GameTracker.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameTracker.Web.Pages.Games;

public class EditModel : PageModel
{
    private readonly IGameRepository _gameRepo;
    private readonly ICategoryRepository _categoryRepo;
    private readonly IStudioRepository _studioRepo;

    public List<Category> Categories { get; set; } = new();
    public List<Studio> Studios { get; set; } = new();

    [BindProperty]
    public EditInput Input { get; set; } = new();

    public class EditInput
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int? ReleaseYear { get; set; }
        public List<int> CategoryIds { get; set; } = new();
        public List<int> StudioIds { get; set; } = new();
    }

    public EditModel(IGameRepository gameRepo, ICategoryRepository categoryRepo, IStudioRepository studioRepo)
    {
        _gameRepo = gameRepo;
        _categoryRepo = categoryRepo;
        _studioRepo = studioRepo;
    }

    public IActionResult OnGet(int id)
    {
        var game = _gameRepo.GetByIdWithRelations(id);
        if (game == null) return NotFound();

        Input = new EditInput
        {
            Id = game.Id,
            Title = game.Title,
            ReleaseYear = game.ReleaseYear,
            CategoryIds = game.Categories.Select(c => c.Id).ToList(),
            StudioIds = game.Studios.Select(s => s.Id).ToList()
        };

        LoadSelectLists();
        return Page();
    }

    public IActionResult OnPost()
    {
        var game = _gameRepo.GetByIdWithRelations(Input.Id);
        if (game == null) return NotFound();

        if (Input.CategoryIds == null || !Input.CategoryIds.Any())
            ModelState.AddModelError("Input.CategoryIds", "Sélectionnez au moins une catégorie.");

        if (string.IsNullOrWhiteSpace(Input.Title))
        {
            ModelState.AddModelError("Input.Title", "Le titre est requis.");
        }

        if (!ModelState.IsValid)
        {
            LoadSelectLists();
            return Page();
        }

        var categories = _categoryRepo.GetAll().Where(c => Input.CategoryIds.Contains(c.Id)).ToList();
        var studios = Input.StudioIds.Count > 0
            ? _studioRepo.GetAll().Where(s => Input.StudioIds.Contains(s.Id)).ToList()
            : new List<Studio>();

        var gameToUpdate = new Game
        {
            Id = Input.Id,
            Title = Input.Title.Trim(),
            ReleaseYear = Input.ReleaseYear,
            Categories = categories,
            Studios = studios
        };

        _gameRepo.Update(Input.Id, gameToUpdate);
        return RedirectToPage("/Games/Index");
    }

    private void LoadSelectLists()
    {
        Categories = _categoryRepo.GetAll();
        Studios = _studioRepo.GetAll();
    }
}
