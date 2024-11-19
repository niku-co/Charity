using NikuAPI.Entities;

namespace NikuAPI.IRepository;

public interface IGoodTypeRepository
{
    Task<IEnumerable<GoodType>> GetAll();
    Task<IEnumerable<int>> GetUpdatedGoodTypesId(string LastImageUpdate);
    Task<byte[]> GetGoodTypeIma12ById(int id);
    Task<byte[]> GetGoodTypeGifById(int id);

}
