using Dapper;
using NikuAPI.Entities;
using NikuAPI.IRepository;
using System.Data.SqlClient;

namespace NikuAPI.Repository;
public class QuoteRepository : IQuoteRepository
{
    private readonly IConfiguration _configuration;
    public QuoteRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<IEnumerable<Quote>> GetAllQuotes()
    {
        var sql = @"select QuoteID, Quote as quote from Quotes where Active = 1";
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        using var connection = new SqlConnection(connectionString);
        var result = await connection.QueryAsync<Quote>(sql);
        return result;
    }

    public async Task<IEnumerable<Quote>> GetQuote(string quoteType)
    {
        //var sql = @"select MessageText from SMS_Send";
        //var connectionString = _configuration.GetConnectionString("DefaultConnection");
        //using var connection = new SqlConnection(connectionString);
        //var messageType = await connection.QueryFirstOrDefaultAsync<string>(sql);


        //sql = $"select QuoteID, Quote from Quotes where Active = 1";
        //if (!string.IsNullOrEmpty(messageType))
        //{
        //    sql += $" AND Topic = N'{messageType}'";
        //}
        //var result = await connection.QueryAsync<Quote>(sql);

        //if (result == null || !result.Any()) 
        //{
        //    sql = $"select QuoteID, Quote from Quotes where Active = 1";
        //    result = await connection.QueryAsync<Quote>(sql);
        //}
        //return result;

        var sql = $"select QuoteID, Quote from Quotes where Active = 1";
        if (!string.IsNullOrEmpty(quoteType))
            sql += $" AND Topic = N'{quoteType}'";

        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        using var connection = new SqlConnection(connectionString);

        var result = await connection.QueryAsync<Quote>(sql);

        if (result == null || !result.Any())
        {
            sql = $"select QuoteID, Quote from Quotes where Active = 1";
            result = await connection.QueryAsync<Quote>(sql);
        }
        return result;
    }
}
