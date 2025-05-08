using Dapper;
using NikuAPI.IRepository;
using System.Data.SqlClient;

namespace NikuAPI.Repository;

public class KioskRepository : IKioskRepository
{
    private readonly IConfiguration _configuration;
    public KioskRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<int> GetKioskId(string ipAddress, string pcName)
    {
        var sql = @"SELECT KioskID FROM KioskManager WHERE Active=1 AND (PC_Name=@pcName)";
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        using var connection = new SqlConnection(connectionString);
        var result = await connection.QueryFirstOrDefaultAsync<int>(sql, new { pcName = pcName });
        if (result > 0)
            return result;
        var sqlBytIp = @"SELECT KioskID FROM KioskManager WHERE Active=1 AND (IP=@ipAddress)";
        result = await connection.QueryFirstOrDefaultAsync<int>(sqlBytIp, new { ipAddress = ipAddress });
        return result;
    }
}
