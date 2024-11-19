using NikuAPI.Entities;

namespace NikuAPI.IRepository;

public interface IQuoteRepository
{
    Task<IEnumerable<Quote>> GetAllQuotes();
    Task<IEnumerable<Quote>> GetQuote(string quoteType);
}
