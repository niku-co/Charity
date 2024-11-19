using NikuAPI.Entities;

namespace NikuAPI.IRepository;

public interface IGoodRepository
{
    Task<IEnumerable<Good>> GetAll();
    Task<byte[]> GetGoodImaById(int id);
    Task<byte[]> GetGoodImaLargeById(int id);

    Task<IEnumerable<int>> GetUpdatedGoodsId(string LastImageUpdate);
    Task<(long, string)> CheckExist(int goodId, int count, int storeId);
}
