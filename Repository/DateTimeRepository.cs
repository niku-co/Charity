using Dapper;
using NikuAPI.IRepository;
using System.Data.SqlClient;

namespace NikuAPI.Repository;

public class DateTimeRepository : IDateTimeRepository
{
    private readonly IConfiguration _configuration;
    public DateTimeRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<string> GetCurrentDateTime()
    {
        var sql = @"Exec GetCurrentDateTime";
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        using var connection = new SqlConnection(connectionString);
        var result = await connection.QueryFirstOrDefaultAsync<string>(sql);
        return result.ToString();
    }
}
