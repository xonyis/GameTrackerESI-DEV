using GameTracker.Models;

namespace GameTracker.Repositories;

public interface ICategoryRepository
{
    List<Category> GetAll();
    Category? GetById(int id);
    bool CanDelete(int categoryId);
    void Add(Category category);
    void Update(int id, Category category);
    void Delete(int id);
}
