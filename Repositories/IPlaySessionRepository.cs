using GameTracker.Models;

namespace GameTracker.Repositories;

public interface IPlaySessionRepository
{
    PlaySession? GetById(int id);
    PlaySession? GetByUserAndGame(int userId, int gameId);
    void Add(PlaySession session);
    void Update(int id, PlaySession session);
    bool UpdateHours(int userId, int gameId, int hoursPlayed);
}
