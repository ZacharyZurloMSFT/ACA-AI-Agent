using System.Runtime.CompilerServices;
using Microsoft.Data.SqlClient;
using ZurlocloudRunningWebAPI.Entities;

using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace ZurlocloudRunningWebAPI.Services;

/// <summary>
/// The database service for querying the Zurlocloud Running database.
/// </summary>
public class DatabaseService : IDatabaseService
{
    /// <summary>
    /// Get all products from the database.
    /// </summary>
    [KernelFunction("get_products")]
    [Description("Get all products.")]
    public async Task<IEnumerable<AllProducts>> GetProducts()
    {
        Console.WriteLine("GetProducts method called.");
        var sql = "SELECT ProductID, ProductName, ProductCategory, Price FROM dbo.AllProducts";   

        using var conn = new SqlConnection(
            connectionString: Environment.GetEnvironmentVariable("SQLAZURECONNSTR_ContosoSuites")!
        );
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        using var reader = await cmd.ExecuteReaderAsync();
        var products = new List<AllProducts>();
        while (await reader.ReadAsync())
        {
            products.Add(new AllProducts
            {
                ProductID = reader.GetInt32(0),
                ProductName = reader.GetString(1),
                ProductCategory = reader.GetString(2),
                Price = reader.GetDecimal(3)
            });
 
        }
        conn.Close();

        return products;
    }


    /// <summary>
    /// Get all stores from the database.
    /// </summary>
    [KernelFunction("get_stores")]
    [Description("Get all stores.")]
    public async Task<IEnumerable<Store>> GetStores()
    {
        Console.WriteLine("GetStores method called.");
        var sql = "SELECT StoreID, City, Country FROM dbo.Store";   

        using var conn = new SqlConnection(
            connectionString: Environment.GetEnvironmentVariable("SQLAZURECONNSTR_ContosoSuites")!
        );
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        using var reader = await cmd.ExecuteReaderAsync();
        var stores = new List<Store>();
        while (await reader.ReadAsync())
        {
            stores.Add(new Store
            {
                StoreID = reader.GetInt32(0),
                City = reader.GetString(1),
                Country = reader.GetString(2)
            });

 
        }
        conn.Close();

        return stores;
    }


    /// <summary>
    /// Get a specific store's products from the database.
    /// </summary>
    [KernelFunction("get_products_for_store")]
    [Description("Get all products for a single store.")]
    public async Task<IEnumerable<StoreProducts>> GetProductsForStore(
        [Description("The ID of the store")] int storeId
        )
    {
        Console.WriteLine("Get all products for single store method called.");
        var sql = "SELECT StoreID, ProductID, ProductName, Price, Quantity FROM dbo.StoreProducts WHERE StoreID = @StoreID";
        using var conn = new SqlConnection(
            connectionString: Environment.GetEnvironmentVariable("SQLAZURECONNSTR_ContosoSuites")!
        );
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@StoreID", storeId);
        using var reader = await cmd.ExecuteReaderAsync();
        var products = new List<StoreProducts>();
        while (await reader.ReadAsync())
        {
            products.Add(new StoreProducts
            {
                StoreID = reader.GetInt32(0),
                ProductID = reader.GetInt32(1),
                ProductName = reader.GetString(2),
                Price = reader.GetDecimal(3),
                Quantity = reader.GetInt32(4)
            });

        }
        conn.Close();

        return products;
    }
    /// <summary>
    /// Get a specific store's products from the database.
    /// </summary>
    [KernelFunction("get_order_details_with_multiple_products")]
    [Description("Get all products for a single store.")]
    public async Task<IEnumerable<MultiProductOrder>> GetOrderDetailsWithMultipleProducts()
    {
        Console.WriteLine("get order details with multiple products method called.");
        List<OrderDetails> orders = new List<OrderDetails>();
        var sql = @"
            SELECT o.OrderID, o.CustomerID, o.StoreID, o.OrderDate, COUNT(od.ProductID) AS ProductCount
            FROM dbo.Orders o
            JOIN dbo.OrderDetails od ON o.OrderID = od.OrderID
            GROUP BY o.OrderID, o.StoreID, o.OrderDate, o.CustomerID
            HAVING COUNT(od.ProductID) > 1;";
        using var conn = new SqlConnection(
            connectionString: Environment.GetEnvironmentVariable("SQLAZURECONNSTR_ContosoSuites")!
        );
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        using var reader = await cmd.ExecuteReaderAsync();
        var multiProductOrders = new List<MultiProductOrder>();
        while (await reader.ReadAsync())
        {
            multiProductOrders.Add(new MultiProductOrder
            {
                OrderID = reader.GetInt32(0),
                CustomerID = reader.GetInt32(1),
                StoreID = reader.GetInt32(2),
                OrderDate = reader.GetDateTime(3),
                ProductCount = reader.GetInt32(4)
            });
        }
        conn.Close();

        return multiProductOrders;
    }


    //create kernel function to get orders by date
    [KernelFunction("get_orders_by_date")]
    [Description("Get all orders for a store after a specified date.")]
    public async Task<IEnumerable<Orders>> GetOrdersByDate(
        [Description("The ID of the store")] int storeId,
        [Description("The minimum date for the orders")] DateTime min_date
        )
    {
        Console.WriteLine("Get orders by date method called.");
        var sql = "SELECT OrderID, CustomerID, StoreID, OrderDate FROM dbo.Orders WHERE StoreID = @StoreID AND OrderDate > @MinDate";
        using var conn = new SqlConnection(
            connectionString: Environment.GetEnvironmentVariable("SQLAZURECONNSTR_ContosoSuites")!
        );
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@StoreID", storeId);
        cmd.Parameters.AddWithValue("@MinDate", min_date);
        using var reader = await cmd.ExecuteReaderAsync();
        var orders = new List<Orders>();
        while (await reader.ReadAsync())
        {
            orders.Add(new Orders
            {
                OrderID = reader.GetInt32(0),
                CustomerID = reader.GetInt32(1),
                StoreID = reader.GetInt32(2),
                OrderDate = reader.GetDateTime(3)
            });
        }
        conn.Close();

        return orders;
    }
}
