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
    /// Get all stores from the database.
    /// </summary>
    [KernelFunction("get_stores")]
    [Description("Get all stores.")]
    public async Task<IEnumerable<Store>> GetStores()
    {
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
    /// Get a specific store's orders from the database.
    /// </summary>
    [KernelFunction]
    [Description("Get all orders for a single store.")]
    public async Task<IEnumerable<Orders>> GetOrdersForStore(
        [Description("The ID of the store")] int storeId
        )
    {
        var sql = "SELECT OrderID, CustomerID, StoreID, OrderDate, TotalAmount FROM dbo.Orders WHERE StoreID = @StoreID";
        using var conn = new SqlConnection(
            connectionString: Environment.GetEnvironmentVariable("SQLAZURECONNSTR_ContosoSuites")!
        );
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@StoreID", storeId);
        using var reader = await cmd.ExecuteReaderAsync();
        var orders = new List<Orders>();
        while (await reader.ReadAsync())
        {
            orders.Add(new Orders
            {
                OrderID = reader.GetInt32(0),
                CustomerID = reader.GetInt32(1),
                StoreID = reader.GetInt32(2),
                OrderDate = reader.GetDateTime(3),
                TotalAmount = reader.GetDecimal(4)
            });

        }
        conn.Close();

        return orders;
    }



    public async Task<IEnumerable<Orders>> GetOrdersWithMultipleProducts()
    {
        var sql = """
            SELECT
                b.OrderID,
                b.CustomerID,
                b.StoreID,
                b.OrderDate,
                b.TotalAmount,
            FROM dbo.Orders b
            WHERE
                (
                    SELECT COUNT(1) 
                    FROM dbo.ProductOrder po 
                    WHERE po.OrderID = o.OrderID
                ) > 1;
            """;
        using var conn = new SqlConnection(
            connectionString: Environment.GetEnvironmentVariable("SQLAZURECONNSTR_ContosoSuites")!
        );
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        using var reader = await cmd.ExecuteReaderAsync();
        var orders = new List<Orders>();
        while (await reader.ReadAsync())
        {
            orders.Add(new Orders
            {
                OrderID = reader.GetInt32(0),
                CustomerID = reader.GetInt32(1),
                StoreID = reader.GetInt32(2),
                OrderDate = reader.GetDateTime(3),
                TotalAmount = reader.GetDecimal(4)
            });

        }
        conn.Close();

        return orders;
    }



    /// <summary>
    /// Get bookings for a specific store that are after a specified date.
    /// </summary>
    public async Task<IEnumerable<Orders>> GetOrdersByDate(int storeId, DateTime dt)
    {
        var sql = "SELECT OrderID, CustomerID, StoreID, OrderDate, TotalAmount FROM dbo.Orders WHERE StoreID = @StoreID AND OrderDate >= @OrderBeginDate";
        using var conn = new SqlConnection(
            connectionString: Environment.GetEnvironmentVariable("SQLAZURECONNSTR_ContosoSuites")!
        );
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@StoreID", storeId);
        cmd.Parameters.AddWithValue("@OrderBeginDate", dt);
        using var reader = await cmd.ExecuteReaderAsync();
        var orders = new List<Orders>();
        while (await reader.ReadAsync())
        {
            orders.Add(new Orders
            {
                OrderID = reader.GetInt32(0),
                CustomerID = reader.GetInt32(1),
                StoreID = reader.GetInt32(2),
                OrderDate = reader.GetDateTime(3),
                TotalAmount = reader.GetDecimal(4)
            });

        }
        conn.Close();

        return orders;
    }
}
