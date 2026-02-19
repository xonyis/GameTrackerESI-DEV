using GameTracker.Models;

namespace GameTracker.Repositories;

public interface IGameRepository
{
    List<Game> GetAll();
    List<Game> GetByUserId(int userId);
    List<Game> GetByUserIdAndCategory(int userId, int categoryId);
    Game? GetByIdWithRelations(int id);
    void Add(Game game);
    void Update(int id, Game game);
    void Delete(int id);
}
