using GameTracker.Data;
using GameTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace GameTracker.Repositories;

public class UserRepository : IUserRepository
{
    private readonly GameTrackerDbContext _context;

    public UserRepository(GameTrackerDbContext context)
    {
        _context = context;
    }

    public List<User> GetAll()
    {
        return _context.Users.AsNoTracking().ToList();
    }

    public User? GetById(int id)
    {
        return _context.Users.AsNoTracking().FirstOrDefault(u => u.Id == id);
    }

    public void Add(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }
}
