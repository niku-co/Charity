using Dapper;
using NikuAPI.Entities;
using NikuAPI.IRepository;
using System.Data.SqlClient;

namespace NikuAPI.Repository; public class CustomerRepository : ICustomerRepository
{
    private readonly IConfiguration _configuration;
    public CustomerRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<int> AddCustomer(Customer customer)
    {
        var sql = @"INSERT INTO [Customers] ([FullName],[Mobile],[MelliCode],[Age],[FatherName], [CompanyCode])OUTPUT INSERTED.CustomerCode
                        VALUES (@FullName, @Mobile, @MelliCode, @Age, @FatherName, @CompanyCode)";
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        using var connection = new SqlConnection(connectionString);
        var result = await connection.QueryFirstOrDefaultAsync<int>(sql,
            new
            {
                FullName = customer.FullName,
                MelliCode = customer.MelliCode,
                Mobile = customer.Mobile,
                Age = customer.Age,
                FatherName = customer.FatherName,
                CompanyCode = customer.CompanyCode,
            });
        return result;
    }

    public async Task<Customer> GetCustomerByPhoneNumber(string phoneNumber)
    {
        var sql = @"SELECT FullName, Mobile, MelliCode, FatherName, Age, CompanyCode FROM Customers WHERE Mobile = @phoneNumber";
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        using var connection = new SqlConnection(connectionString);
        var result = await connection.QueryFirstOrDefaultAsync<Customer>(sql, new { phoneNumber = phoneNumber });
        return result;
    }

    public async Task<int> GetCustomerOrdersCount(CustomerCountDTO customerDTO)
    {
        var sql = $"SELECT COUNT(*) FROM Orders join Customers on Orders.CustomerID = Customers.CustomerID " +
                              $"WHERE Orders.GoodIDs='{{{customerDTO.GoodId}}}' and MelliCode = '{customerDTO.NationalCode}'" +
                              $"and OrderDate <= {customerDTO.EndDate} and OrderDate >= {customerDTO.StartDate}";
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        using var connection = new SqlConnection(connectionString);
        var result = await connection.QueryFirstOrDefaultAsync<int>(sql);
        return result;
    }
}
