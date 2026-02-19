using GameTracker.Models;
using GameTracker.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameTracker.Web.Pages.Studios;

public class IndexModel : PageModel
{
    private readonly IStudioRepository _studioRepo;

    public List<Studio> Studios { get; set; } = new();

    public IndexModel(IStudioRepository studioRepo)
    {
        _studioRepo = studioRepo;
    }

    public void OnGet()
    {
        Studios = _studioRepo.GetAll();
    }
}
