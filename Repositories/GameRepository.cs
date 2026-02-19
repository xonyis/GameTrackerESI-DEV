using GameTracker.Data;
using GameTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace GameTracker.Repositories;

public class GameRepository : IGameRepository
{
    private readonly GameTrackerDbContext _context;

    public GameRepository(GameTrackerDbContext context)
    {
        _context = context;
    }

    public List<Game> GetAll()
    {
        return _context.Games
            .AsNoTracking()
            .Include(g => g.Categories)
            .Include(g => g.Studios)
            .ToList();
    }

    public List<Game> GetByUserId(int userId)
    {
        return _context.Games
            .AsNoTracking()
            .Include(g => g.Categories)
            .Include(g => g.Studios)
            .Include(g => g.PlaySessions.Where(ps => ps.UserId == userId))
            .Where(g => g.PlaySessions.Any(ps => ps.UserId == userId))
            .ToList();
    }

    public List<Game> GetByUserIdAndCategory(int userId, int categoryId)
    {
        return _context.Games
            .AsNoTracking()
            .Include(g => g.Categories)
            .Include(g => g.Studios)
            .Include(g => g.PlaySessions.Where(ps => ps.UserId == userId))
            .Where(g => g.Categories.Any(c => c.Id == categoryId) && g.PlaySessions.Any(ps => ps.UserId == userId))
            .ToList();
    }

    public Game? GetByIdWithRelations(int id)
    {
        return _context.Games
            .AsNoTracking()
            .Include(g => g.Categories)
            .Include(g => g.Studios)
            .Include(g => g.PlaySessions)
                .ThenInclude(ps => ps.User)
            .FirstOrDefault(g => g.Id == id);
    }

    public void Add(Game game)
    {
        if (game.Categories.Any())
        {
            var categoryIds = game.Categories.Select(c => c.Id).ToList();
            var trackedCategories = _context.Categories.Where(c => categoryIds.Contains(c.Id)).ToList();
            game.Categories = trackedCategories;
        }
        if (game.Studios.Any())
        {
            var studioIds = game.Studios.Select(s => s.Id).ToList();
            var trackedStudios = _context.Studios.Where(s => studioIds.Contains(s.Id)).ToList();
            game.Studios = trackedStudios;
        }
        _context.Games.Add(game);
        _context.SaveChanges();
    }

    public void Update(int id, Game game)
    {
        var toUpdate = _context.Games.Include(g => g.Categories).Include(g => g.Studios).FirstOrDefault(g => g.Id == id);
        if (toUpdate == null) return;

        toUpdate.Title = game.Title;
        toUpdate.ReleaseYear = game.ReleaseYear;

        toUpdate.Categories.Clear();
        if (game.Categories.Any())
        {
            var categoryIds = game.Categories.Select(c => c.Id).ToList();
            var categories = _context.Categories.Where(c => categoryIds.Contains(c.Id)).ToList();
            foreach (var c in categories)
                toUpdate.Categories.Add(c);
        }

        toUpdate.Studios.Clear();
        if (game.Studios.Any())
        {
            var studioIds = game.Studios.Select(s => s.Id).ToList();
            var studios = _context.Studios.Where(s => studioIds.Contains(s.Id)).ToList();
            foreach (var s in studios)
                toUpdate.Studios.Add(s);
        }

        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var game = _context.Games.FirstOrDefault(g => g.Id == id);
        if (game == null) return;

        var sessions = _context.PlaySessions.Where(ps => ps.GameId == id).ToList();
        _context.PlaySessions.RemoveRange(sessions);
        _context.Games.Remove(game);
        _context.SaveChanges();
    }
}
