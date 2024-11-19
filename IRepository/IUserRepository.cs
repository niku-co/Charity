using NikuAPI.Entities;

namespace NikuAPI.IRepository;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsers();
}
