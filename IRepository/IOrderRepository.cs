using NikuAPI.Entities;

namespace NikuAPI.IRepository;

public interface IOrderRepository
{
    Task DeleteOrder(string orderId);
    Task<Guid> GetOrderById(string orderId);
    Task<OrderResponse> SaveOrder(SaveOrder order);
    Task<Result<IEnumerable<OrderGood>, string>> UpdateDelivery(string code);
    Task<string> GetLastCashTransaction(string pcName);
}
