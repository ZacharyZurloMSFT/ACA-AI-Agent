using global::ZurlocloudRunningWebAPI.Entities;

namespace ZurlocloudRunningWebAPI.Services;

public interface IDatabaseService
{
    Task<IEnumerable<Store>> GetStores();
    Task<IEnumerable<Orders>> GetOrdersForStore(int storeId);
    Task<IEnumerable<Orders>> GetOrdersByDate(int storeId, DateTime dt);
    Task<IEnumerable<Orders>> GetOrdersWithMultipleProducts();
}