namespace NikuAPI.IRepository;

public interface IDateTimeRepository
{
    Task<string> GetCurrentDateTime();
}
