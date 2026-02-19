using GameTracker.Data;
using GameTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace GameTracker.Repositories;

public class PlaySessionRepository : IPlaySessionRepository
{
    private readonly GameTrackerDbContext _context;

    public PlaySessionRepository(GameTrackerDbContext context)
    {
        _context = context;
    }

    public PlaySession? GetById(int id)
    {
        return _context.PlaySessions
            .AsNoTracking()
            .Include(ps => ps.Game)
            .Include(ps => ps.User)
            .FirstOrDefault(ps => ps.Id == id);
    }

    public PlaySession? GetByUserAndGame(int userId, int gameId)
    {
        return _context.PlaySessions
            .AsNoTracking()
            .Include(ps => ps.Game)
            .Include(ps => ps.User)
            .FirstOrDefault(ps => ps.UserId == userId && ps.GameId == gameId);
    }

    public void Add(PlaySession session)
    {
        _context.PlaySessions.Add(session);
        _context.SaveChanges();
    }

    public void Update(int id, PlaySession session)
    {
        var toUpdate = _context.PlaySessions.FirstOrDefault(ps => ps.Id == id);
        if (toUpdate == null) return;
        toUpdate.Date = session.Date;
        toUpdate.HoursPlayed = session.HoursPlayed;
        _context.SaveChanges();
    }

    public bool UpdateHours(int userId, int gameId, int hoursPlayed)
    {
        var session = _context.PlaySessions
            .FirstOrDefault(ps => ps.UserId == userId && ps.GameId == gameId);
        if (session == null) return false;
        session.HoursPlayed = hoursPlayed;
        _context.SaveChanges();
        return true;
    }
}
