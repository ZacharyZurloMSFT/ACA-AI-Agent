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
    [KernelFunction("get_products")]
    [Description("Get all products.")]
    public async Task<IEnumerable<AllProducts>> GetProducts()
    {
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
    [Description("Get all products for a single store.")]
    public async Task<IEnumerable<StoreProducts>> GetProductsForStore(
        [Description("The ID of the store")] int storeId
        )
    {
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






}
