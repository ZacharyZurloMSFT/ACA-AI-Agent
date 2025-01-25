using System.ComponentModel; 
using ZurlocloudRunningWebAPI.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.SemanticKernel;
// Exercise 5 Task 2 TODO #5: Add a library references support Semantic Kernel.

namespace ZurlocloudRunningWebAPI.Plugins
{
    /// <summary>
    /// The maintenance request plugin for creating and saving maintenance requests.
    /// </summary>
    public class InventoryRequestPlugin(CosmosClient cosmosClient)
    {
        private readonly CosmosClient _cosmosClient = cosmosClient;

        // Exercise 5 Task 2 TODO #6: Add KernelFunction and Description descriptors to the function.
        // The function should be named "create_maintenance_request" and it should have a description
        // the accurately describes the purpose of the function, such as "Creates a new maintenance request for a hotel."

        // Exercise 5 Task 2 TODO #7: Add Kernel as the first parameter to the function.
        /// <summary>
        /// Creates a new maintenance request for a hotel.
        /// </summary>
        [KernelFunction("create_maintenance_request")]
        [Description("Creates a new maintenance request for a hotel.")]
        public async Task<InventoryRequest> CreateInventoryRequest(Kernel kernel, int StoreID, int ProductID, string Details, int? RoomNumber, string? location)
        {
            try
            {
                Console.WriteLine($"Creating a new maintenance request for the product ID: {ProductID} in store: {StoreID}.");

                var request = new InventoryRequest
                {
                    id = Guid.NewGuid().ToString(),
                    store_id = StoreID,
                    product_id = ProductID,
                    details = Details,
                    source = "customer",
                    location = location
                };
                return request;
            }
            catch (Exception ex)
            {
                throw new Exception($"An exception occurred while generating a new maintenance request: {ex}");
            }
        }

        // Exercise 5 Task 2 TODO #8: Add KernelFunction and Description descriptors to the function.
        // The function should be named "save_maintenance_request" and it should have a description
        // the accurately describes the purpose of the function, such as "Saves a maintenance request to the database for a hotel."

        // Exercise 5 Task 2 TODO #9: Add Kernel as the first parameter to the function.
        /// <summary>
        /// Saves a maintenance request to the database for a hotel.
        /// </summary>
        [KernelFunction("save_maintenance_request")]
        [Description("Saves a maintenance request to the database for a hotel.")]

        public async Task SaveInventoryRequest(Kernel kernel, InventoryRequest inventoryRequest)
        {
            var db = _cosmosClient.GetDatabase("ZurlocloudRunning");
            var container = db.GetContainer("InventoryRequests");

            var response = await container.CreateItemAsync(inventoryRequest, new PartitionKey(inventoryRequest.store_id));
        }
    }
}
