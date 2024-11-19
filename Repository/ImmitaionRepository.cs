using Dapper;
using NikuAPI.Entities;
using NikuAPI.IRepository;
using System.Data.SqlClient;

namespace NikuAPI.Repository;

public class ImmitaionRepository : IImitationRepository
{
    private readonly IConfiguration _configuration;
    public ImmitaionRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<IEnumerable<Immitation>> GetAll()
    {
        var sql = @"select CompanyCode, CompanyName from Company_Customer";
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        using var connection = new SqlConnection(connectionString);
        var result = await connection.QueryAsync<Immitation>(sql);

        return result;
    }
}
