using NikuAPI.Entities;

namespace NikuAPI.IRepository;

public interface IStoreRepository
{
    Task<int> GetStoreId();
    Task<IEnumerable<StoreId>> GetStoreIds();
    Task<bool> GetOrderSetting();
    Task<Store> GetStore();
}
