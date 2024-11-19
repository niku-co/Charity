using Dapper;
using NikuAPI.Entities;
using NikuAPI.IRepository;
using System.Data.SqlClient;

namespace NikuAPI.Repository;

public class StoreRepository : IStoreRepository
{
    private readonly IConfiguration _configuration;
    public StoreRepository(IConfiguration configuration)
    {
        _configuration= configuration;
    }

    public Task<Store> GetStore()
    {
        var sql = @"SELECT * FROM Store";
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        var connection = new SqlConnection(connectionString);
        var result = connection.QueryFirstOrDefaultAsync<Store>(sql);
        return result;
    }

    public Task<int> GetStoreId()
    {
        var sql = @"SELECT StoreID as Id FROM Good_Stores WHERE Active=1";
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        var connection = new SqlConnection(connectionString);
        var result = connection.QueryFirstOrDefaultAsync<int>(sql);
        return result;
    }

    public Task<IEnumerable<StoreId>> GetStoreIds()
    {
        var sql = @"SELECT StoreID as Id FROM Good_Stores WHERE Active=1";
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        var connection = new SqlConnection(connectionString);
        var result = connection.QueryAsync<StoreId>(sql);
        return result;
    }


    public async Task<bool> GetOrderSetting()
    {
        bool updateDelivery = false;

        var sql = @"SELECT OrderSetting FROM Store";
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        var connection = new SqlConnection(connectionString);
        var result = await connection.QueryFirstOrDefaultAsync<byte[]>(sql);
        updateDelivery = (result[2] & (1U << 1)) != 0;
        return updateDelivery;
    }
}
