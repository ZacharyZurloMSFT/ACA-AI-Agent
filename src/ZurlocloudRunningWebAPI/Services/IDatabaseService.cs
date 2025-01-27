using global::ZurlocloudRunningWebAPI.Entities;

namespace ZurlocloudRunningWebAPI.Services;

public interface IDatabaseService
{
    Task<IEnumerable<AllProducts>> GetProducts();
    Task<IEnumerable<Store>> GetStores();
    Task<IEnumerable<StoreProducts>> GetProductsForStore(int storeId);
    // Task<IEnumerable<Orders>> GetOrdersByDate(int storeId, DateTime dt);
    // Task<IEnumerable<Orders>> GetOrdersWithMultipleProducts();
}