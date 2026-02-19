using GameTracker.Data;
using GameTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace GameTracker.Repositories;

public class StudioRepository : IStudioRepository
{
    private readonly GameTrackerDbContext _context;

    public StudioRepository(GameTrackerDbContext context)
    {
        _context = context;
    }

    public List<Studio> GetAll()
    {
        return _context.Studios.AsNoTracking().ToList();
    }

    public Studio? GetById(int id)
    {
        return _context.Studios.AsNoTracking().FirstOrDefault(s => s.Id == id);
    }

    public void Add(Studio studio)
    {
        _context.Studios.Add(studio);
        _context.SaveChanges();
    }

    public void Update(int id, Studio studio)
    {
        var toUpdate = _context.Studios.FirstOrDefault(s => s.Id == id);
        if (toUpdate == null) return;
        toUpdate.Name = studio.Name;
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var studio = _context.Studios.FirstOrDefault(s => s.Id == id);
        if (studio == null) return;
        _context.Studios.Remove(studio);
        _context.SaveChanges();
    }
}
