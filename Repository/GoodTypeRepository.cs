using Dapper;
using NikuAPI.Entities;
using NikuAPI.IRepository;
using System.Data.SqlClient;

namespace NikuAPI.Repository;

public class GoodTypeRepository : IGoodTypeRepository
{
    private readonly IConfiguration _configuration;
    public GoodTypeRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<IEnumerable<GoodType>> GetAll()
    {
        var sql = @"SELECT GoodTypeID, Topic, CategoryID, ActiveKiosk, LayOutIndex, LayOutID, KioskIDs, Secondary,
                        PageManagement, OfferManagement, Settings, Message
                        FROM GoodTypes WHERE ActiveKiosk=1 AND Active=1";
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        using var connection = new SqlConnection(connectionString);
        var result = await connection.QueryAsync<GoodType>(sql);

        return result;
    }

    public async Task<IEnumerable<int>> GetUpdatedGoodTypesId(string lastImageUpdate)
    {
        var sql = @"SELECT GoodTypeID FROM GoodTypes WHERE ActiveKiosk=1 and LastImageUpdate > @lastImageUpdate";
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        using var connection = new SqlConnection(connectionString);
        var result = await connection.QueryAsync<int>(sql, new { lastImageUpdate });

        return result;
    }

    public async Task<byte[]> GetGoodTypeIma12ById(int id)
    {
        var sql = @"SELECT Ima12 FROM GoodTypes WHERE ActiveKiosk=1 and GoodTypeID = @id";
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        using var connection = new SqlConnection(connectionString);
        var result = await connection.QueryFirstOrDefaultAsync<byte[]>(sql, new { id });

        return result;
    }

    public async Task<byte[]> GetGoodTypeGifById(int id)
    {
        var sql = @"SELECT GIF FROM GoodTypes WHERE ActiveKiosk=1 and GoodTypeID = @id";
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        using var connection = new SqlConnection(connectionString);
        var result = await connection.QueryFirstOrDefaultAsync<byte[]>(sql, new { id });

        return result;
    }
}
