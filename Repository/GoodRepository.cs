using Dapper;
using NikuAPI.Entities;
using NikuAPI.IRepository;
using System.Data;
using System.Data.SqlClient;

namespace NikuAPI.Repository;

public class GoodRepository : IGoodRepository
{
    private readonly IConfiguration _configuration;
    public GoodRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<(long, string)> CheckExist(int goodId, int count, int StoreId)
    {
        long price = 0;
        string message = "";
        goodId = goodId * 10 + 1;
        var parameters = new DynamicParameters();
        parameters.Add("@StuffID", goodId);
        parameters.Add("@StuffStoreID", StoreId);
        parameters.Add("@StuffQuantity", count);
        parameters.Add("@FactureID", null);
        parameters.Add("@toID", null);
        parameters.Add("@RecieveDate", null);
        parameters.Add("@RecieveTime", null);
        parameters.Add("@Insert_Read", 0);
        parameters.Add("@Price", dbType: DbType.Int64, direction: ParameterDirection.Output);
        parameters.Add("@Message", dbType: DbType.String, size:1500, direction: ParameterDirection.Output);

        var sql = @"InsertIngredientStore";
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        using var connection = new SqlConnection(connectionString);
        var result = await connection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);

        price = parameters.Get<long>("@Price");
        message = parameters.Get<string>("@Message");

        return (price, message);
    }

    public async Task<IEnumerable<Good>> GetAll()
    {
        var sql = @"SELECT ChildIDs, GoodID as ProductId, GoodTypeID as CategoryId, Goods.Topic, Units.Topic as Unit,
                        Description, Price, OldPrice, ActiveKiosk as Active,LayOutIndex as AccountIndex,
                        LayOutIndexKiosk as LayoutIndex FROM Goods left join Units on Goods.UnitID = Units.UnitID
                        WHERE ActiveKiosk=1";
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        using var connection = new SqlConnection(connectionString);
        var result = await connection.QueryAsync<Good>(sql);

        return result;
    }

    public async Task<IEnumerable<int>> GetUpdatedGoodsId(string LastImageUpdate)
    {
        var sql = @"SELECT GoodID FROM Goods WHERE ActiveKiosk=1 and LastImageUpdate > @LastImageUpdate";
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        using var connection = new SqlConnection(connectionString);
        var result = await connection.QueryAsync<int>(sql, new { LastImageUpdate });

        return result;
    }

    public async Task<byte[]> GetGoodImaById(int id)
    {
        var sql = @"SELECT Ima FROM Goods WHERE ActiveKiosk=1 and GoodID = @id";
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        using var connection = new SqlConnection(connectionString);
        var result = await connection.QueryFirstOrDefaultAsync<byte[]>(sql, new { id });

        return result;
    }

    public async Task<byte[]> GetGoodImaLargeById(int id)
    {
        var sql = @"SELECT Ima_Large FROM Goods WHERE ActiveKiosk=1 and GoodID = @id";
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        using var connection = new SqlConnection(connectionString);
        var result = await connection.QueryFirstOrDefaultAsync<byte[]>(sql, new { id });

        return result;
    }
}
