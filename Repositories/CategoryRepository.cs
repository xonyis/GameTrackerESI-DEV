using GameTracker.Data;
using GameTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace GameTracker.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly GameTrackerDbContext _context;

    public CategoryRepository(GameTrackerDbContext context)
    {
        _context = context;
    }

    public List<Category> GetAll()
    {
        return _context.Categories.AsNoTracking().ToList();
    }

    public Category? GetById(int id)
    {
        return _context.Categories.AsNoTracking().FirstOrDefault(c => c.Id == id);
    }

    public bool CanDelete(int categoryId)
    {
        var gamesWithCategory = _context.Games
            .Include(g => g.Categories)
            .Where(g => g.Categories.Any(c => c.Id == categoryId))
            .ToList();
        return gamesWithCategory.All(g => g.Categories.Count > 1);
    }

    public void Add(Category category)
    {
        _context.Categories.Add(category);
        _context.SaveChanges();
    }

    public void Update(int id, Category category)
    {
        var toUpdate = _context.Categories.FirstOrDefault(c => c.Id == id);
        if (toUpdate == null) return;
        toUpdate.Name = category.Name;
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        if (!CanDelete(id))
            throw new InvalidOperationException("Impossible de supprimer : certains jeux n'ont que cette catégorie. Chaque jeu doit avoir au moins une catégorie.");

        var category = _context.Categories.FirstOrDefault(c => c.Id == id);
        if (category == null) return;

        var gamesWithCategory = _context.Games
            .Include(g => g.Categories)
            .Where(g => g.Categories.Any(c => c.Id == id))
            .ToList();

        foreach (var game in gamesWithCategory)
        {
            var catToRemove = game.Categories.First(c => c.Id == id);
            game.Categories.Remove(catToRemove);
        }

        _context.Categories.Remove(category);
        _context.SaveChanges();
    }
}
