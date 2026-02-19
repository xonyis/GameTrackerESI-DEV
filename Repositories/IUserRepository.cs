using GameTracker.Models;

namespace GameTracker.Repositories;

public interface IUserRepository
{
    List<User> GetAll();
    User? GetById(int id);
    void Add(User user);
}
