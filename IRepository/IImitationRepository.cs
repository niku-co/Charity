using NikuAPI.Entities;

namespace NikuAPI.IRepository;

public interface IImitationRepository
{
    Task<IEnumerable<Immitation>> GetAll();
}
