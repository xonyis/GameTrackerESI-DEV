using GameTracker.Models;
using GameTracker.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GameTracker.Web.Pages.Users;

public class IndexModel : PageModel
{
    private readonly IUserRepository _userRepo;

    public List<User> Users { get; set; } = new();

    public IndexModel(IUserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    public void OnGet()
    {
        Users = _userRepo.GetAll();
    }
}
