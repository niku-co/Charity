using Dapper;
using FluentValidation;
using NikuAPI.Entities;
using NikuAPI.IRepository;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace NikuAPI.Repository;

public class OrderRepository(IConfiguration configuration, IValidator<Order> validator, ILogger<OrderRepository> logger) : IOrderRepository
{
    private readonly IConfiguration _configuration = configuration;
    private readonly IValidator<Order> _validator = validator;
    private readonly ILogger<OrderRepository> _logger = logger;

    public async Task DeleteOrder(string orderId)
    {
        var sql = @"DELETE FROM Orders WHERE OrderID = @orderId";
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        using var connection = new SqlConnection(connectionString);
        var result = await connection.QuerySingleOrDefaultAsync(sql, new { orderId = orderId });
    }

    public async Task<string> GetLastCashTransaction(string pcName)
    {
        var sql = @"
            SELECT  TOP(1) Orders.OrderNumber,Order_PayBill.PayBillTypeID,PayBillTypes.Topic, Orders.Prices,Orders.Numbers, Goods.Topic,KioskManager.PC_Name 
            FROM Orders INNER JOIN
            Order_PayBill ON Orders.OrderID = Order_PayBill.OrderID
			INNER JOIN PayBillTypes on  Order_PayBill.PayBillTypeID = PayBillTypes.PayBillTypeID
			INNER JOIN Goods ON CAST(REPLACE(REPLACE(Orders.GoodIds, '{', ''), '}', '') AS int) = Goods.GoodID
            INNER JOIN KioskManager on Orders.PersonalID_KioskID = KioskManager.KioskID
			WHERE (KioskManager.PC_Name = @pcName  ) order by Orders.OrderDate DESC, Orders.OrderTime desc, Orders.OrderNumber desc";
        var connectionString = _configuration.GetConnectionString("DefaultConnection");

        _logger.LogInformation("Connection string: {ConnectionString}", connectionString);

        try
        {
            _logger.LogInformation("Opening SQL connection.");
            using var connection = new SqlConnection(connectionString);
            _logger.LogInformation("Executing SQL query to fetch the last cash transaction.");
            var result = await connection.QuerySingleOrDefaultAsync<dynamic>(sql, new { pcName });

            if (result == null)
            {
                _logger.LogWarning("No cash transaction found for the entered device");
                return null;
            }


            _logger.LogInformation("Serializing the query result to JSON.");
            string jsonResult = JsonConvert.SerializeObject(result);
            _logger.LogInformation("JSON result: {JsonResult}", jsonResult);

            return jsonResult;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while executing the query.");
            throw;
        }
    }

    public async Task<Guid> GetOrderById(string orderId)
    {
        var sql = @"SELECT OrderID FROM Orders WHERE OrderID = @orderId";
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        using var connection = new SqlConnection(connectionString);
        var result = await connection.QuerySingleOrDefaultAsync<Guid>(sql, new { orderId = orderId });
        return result;
    }

    public async Task<OrderResponse> SaveOrder(SaveOrder order)
    {
        var sql = @"SaveOrder";

        double.TryParse(order.Number, out double count);
        double.TryParse(order.Price, out double price);

        var parameters = new DynamicParameters();

        parameters.Add("@ScoopID", null);
        parameters.Add("@BespokenID", null);
        parameters.Add("@PackingID", null);
        parameters.Add("@DiscountPers", null);
        parameters.Add("@Taxes", null);
        parameters.Add("@StoreIDs", null);
        parameters.Add("@Address", null);
        parameters.Add("@NetID", null);
        parameters.Add("@CourierID", null);
        parameters.Add("@MarketerCode", null);
        parameters.Add("@MarketerShare", null);
        parameters.Add("@LedgerID", null);
        parameters.Add("@GuestNumber", null);
        parameters.Add("@Description", null);
        parameters.Add("@GoodID", $"{{{order.GoodID}}}");
        parameters.Add("@GoodTypeID", $"{{{order.GoodTypeID}}}");
        parameters.Add("@Price", $"{{{price / count}}}");
        parameters.Add("@Number", $"{{{order.Number}}}".Replace("/", "."));
        parameters.Add("@OrderID", order.OrderID);
        parameters.Add("@Tax", 0);
        parameters.Add("@Mobile", order.Mobile ?? null);
        parameters.Add("@OrderTypeID", 2);
        parameters.Add("@PayBill", order.PayBill);
        parameters.Add("@MelliCode", order.MelliCode ?? null);
        parameters.Add("@CustomerName", order.CustomerName ?? null);
        parameters.Add("@Discount", 0);
        parameters.Add("@DeliveryTypeID", order.DeliveryTypeID);
        parameters.Add("@SaloonID", order.SaloonID == -1 ? null : order.SaloonID);
        parameters.Add("@CustomerCode", order.CustomerCode > 0 ? order.CustomerCode : null);
        parameters.Add("@PersonalID", order.PersonalID);
        parameters.Add("@PagerNumber", order.PagerNumber == -1 ? null : order.PagerNumber);
        parameters.Add("@StoreID", order.StoreID == -1 ? null : order.StoreID);
        parameters.Add("@Payments", order.Payments);
        parameters.Add("@CourierCost", 0);
        parameters.Add("@CompanyID", order.CompanyID ?? null); ;
        parameters.Add("@Reserved", order.Reserved);
        parameters.Add("@OrderDate", order.OrderDate);
        parameters.Add("@UpdateDelivery", order.UpdateDelivery);
        parameters.Add("@OrderNumber", order.OrderNumber);

        try
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            if (connectionString!.IndexOf("Connect Timeout") == -1)
            {
                connectionString = connectionString.TrimEnd(';') + ";Connect Timeout=10;";
            }
            var connection = new SqlConnection(connectionString);

            var result = await connection.QueryFirstOrDefaultAsync(sql, parameters, commandType: CommandType.StoredProcedure, commandTimeout:10);
            var data = (IDictionary<string, object>)result;

            OrderResponse response = new()
            {
                OrderNumber = (int)data.ElementAt(0).Value,
                OrderTime = ((int)data.ElementAt(1).Value).ToString(),
                OrderID = (Guid)data.ElementAt(2).Value,
                OrderDate = ((int)data.ElementAt(4).Value).ToString()
            };

            return response;

        }
        catch (Exception ex)
        {
            return null;
        }


    }

    public async Task<Result<IEnumerable<OrderGood>, string>> UpdateDelivery(string code)
    {
        var orderDate = code[..8];
        var orderNumber = code[12..];

        var sql = @"SELECT * FROM Orders WHERE OrderNumber = @orderNumber AND OrderDate = @orderDate";
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        using var connection = new SqlConnection(connectionString);

        var order = await connection.QuerySingleOrDefaultAsync<Order>(sql, new { orderNumber, orderDate });
        if (order == null)
        {
            return "سفارشی یافت نشد!";
        }

        var validationResult = _validator.Validate(order);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                return error.ErrorMessage;
            }
        }

        var goodIds = order.GoodIDs.Replace("{", "").Replace("}", "").Split(",");
        var numbers = order.Numbers.Replace("{", "").Replace("}", "").Split(",");

        List<OrderGood> goods = [];
        for (int i = 0; i < goodIds.Length; i++)
        {
            sql = @"SELECT GoodID, Topic FROM Goods WHERE GoodID = @goodId";
            var good = await connection.QuerySingleOrDefaultAsync<OrderGood>(sql, new { goodId = goodIds[i] });
            if (good != null)
            {
                good.Numbers = numbers[i];
                goods.Add(good);
            }
        }

        sql = @"UpdateDelivery";

        var parameters = new DynamicParameters();

        parameters.Add("@OrderID", order.OrderID);
        parameters.Add("@RecordDate", 0);


        var result = await connection.QueryFirstOrDefaultAsync(sql, parameters, commandType: CommandType.StoredProcedure);

        if (result != null)
        {
            var data = (IDictionary<string, object>)result;
            return ((string)data.ElementAt(0).Value).ToString();
        }
        return goods;
    }
}
