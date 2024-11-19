using Dapper;
using NikuAPI.Entities;
using NikuAPI.IRepository;
using System.Data.SqlClient;

namespace NikuAPI.Repository;
public class UserRepository : IUserRepository
{
    private readonly IConfiguration _configuration;
    public UserRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        var sql = @"SELECT FullName, PassWord FROM UserManager";
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        using var connection = new SqlConnection(connectionString);
        var result = await connection.QueryAsync<User>(sql);

        return result;
    }
}
