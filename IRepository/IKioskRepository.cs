namespace NikuAPI.IRepository;

public interface IKioskRepository
{
    Task<int> GetKioskId(string ipAddress, string pcName);
}
