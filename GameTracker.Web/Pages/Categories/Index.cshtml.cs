using GameTracker.Models;
using GameTracker.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameTracker.Web.Pages.Categories;

public class IndexModel : PageModel
{
    private readonly ICategoryRepository _categoryRepo;

    public List<Category> Categories { get; set; } = new();

    public IndexModel(ICategoryRepository categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }

    public void OnGet()
    {
        Categories = _categoryRepo.GetAll();
    }
}
