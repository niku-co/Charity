using NikuAPI.Entities;
using NikuAPI.IRepository;
using System.Security.Cryptography;
using System.Text;

namespace NikuAPI.Services;

public interface IUserService
{
    Task<User> Authenticate(string username, string password);
    Task<IEnumerable<User>> GetAll();
}

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }


    public async Task<User> Authenticate(string username, string password)
    {
        byte[] encodedPassword = Encoding.Unicode.GetBytes(password);
        byte[] hash = (CryptoConfig.CreateFromName("MD5") as HashAlgorithm).ComputeHash(encodedPassword);

        var users = await GetAll();
        var user = users.SingleOrDefault(x => x.FullName == username && x.PassWord.SequenceEqual(hash));

        if (user == null)
            return null;

        return user;
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return await _repository.GetAllUsers();
    }
}
