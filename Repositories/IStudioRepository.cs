using GameTracker.Models;

namespace GameTracker.Repositories;

public interface IStudioRepository
{
    List<Studio> GetAll();
    Studio? GetById(int id);
    void Add(Studio studio);
    void Update(int id, Studio studio);
    void Delete(int id);
}
