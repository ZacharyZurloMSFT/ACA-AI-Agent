using Azure;
using Azure.AI.OpenAI;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using OpenAI.Embeddings;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using DotNetEnv;

namespace ZurlocloudRunning.Functions
{
    /// <summary>
    /// A function that listens for changes to inventory requests in Cosmos DB and generates vector embeddings for new requests.
    /// </summary>
    public class CosmosChangeFeedVectorization
    {
        private readonly ILogger _logger;
        private readonly EmbeddingClient _embeddingClient;
        const string DatabaseName = "ZurlocloudRunning";
        const string ContainerName = "InventoryRequests";

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosChangeFeedVectorization"/> class.
        /// </summary>
        /// <param name="loggerFactory">The logger factory. This comes from Program.cs as the tie-in to Application Insights.</param>
        /// <exception cref="ArgumentNullException">Thrown if necessary configuration settings are missing.</exception>
        public CosmosChangeFeedVectorization(ILoggerFactory loggerFactory)
        {
            Env.Load();
            var endpointUrl = Environment.GetEnvironmentVariable("AzureOpenAIEndpoint");
            if (string.IsNullOrEmpty(endpointUrl))
                throw new ArgumentNullException("AzureOpenAIEndpoint", "AzureOpenAIEndpoint is required to run this function.");

            var azureOpenAIKey = Environment.GetEnvironmentVariable("AzureOpenAIKey");
            if (string.IsNullOrEmpty(azureOpenAIKey))
                throw new ArgumentNullException("AzureOpenAIKey", "AzureOpenAIKey is required to run this function.");

            var deploymentName = Environment.GetEnvironmentVariable("EmbeddingDeploymentName");
            if (string.IsNullOrEmpty(deploymentName))
                throw new ArgumentNullException("EmbeddingDeploymentName", "EmbeddingDeploymentName is required to run this function.");

            _logger = loggerFactory.CreateLogger<CosmosChangeFeedVectorization>();
            var oaiEndpoint = new Uri(endpointUrl);
            var credentials = new AzureKeyCredential(azureOpenAIKey);
            var openAIClient = new AzureOpenAIClient(oaiEndpoint, credentials);
            _embeddingClient = openAIClient.GetEmbeddingClient(deploymentName);   
        }

        /// <summary>
        /// Listens for changes to inventory requests in Cosmos DB and generates vector embeddings for new requests.
        /// </summary>
        [Function("VectorizeInventoryRequests")]
        [CosmosDBOutput(DatabaseName, ContainerName, Connection = "CosmosDBConnectionString")]
        public object Run([CosmosDBTrigger(
            databaseName: DatabaseName,
            containerName: ContainerName,
            Connection = "CosmosDBConnectionString",
            LeaseContainerName = "leases",
            CreateLeaseContainerIfNotExists = true)] IReadOnlyList<InventoryRequest> input)
        {
            var documentsToVectorize = input.Where(t => t.Type != "Vectorized");
            if (documentsToVectorize.Count() == 0) return null;

            foreach (var request in documentsToVectorize)
            {
                try
                {
                    // Combine the hotel and details fields into a single string for embedding.
                    var request_text = $"Store: {request.Store}\n Request Details: {request.Details}";
                    // Generate a vector for the inventory request.
                    var embedding = _embeddingClient.GenerateEmbedding(request_text);
                    var requestVector = embedding.Value.Vector;

                    // Add the vector embeddings to the inventory request and mark it as vectorized.
                    request.RequestVector = requestVector.ToArray();
                    request.Type = "Vectorized";
                    _logger.LogInformation($"Generated vector embeddings for inventory request {request.Id}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error generating vector embeddings for inventory request {request.Id}");
                }
            }

            // Write the updated documents back to Cosmos DB.
            return input;
        }
    }

    /// <summary>
    /// Represents an inventory request.
    /// </summary>
    public class InventoryRequest
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("store_id")]
        public int HotelId {get;set;}
        

        [JsonPropertyName("product_id")]
        public string? Type { get; set; }
        
        [JsonPropertyName("store")]
        public string Store { get; set; }

        [JsonPropertyName("product_name")]
        public string ProductName { get; set; }

        [JsonPropertyName("source")]
        public string Source { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("details")]
        public string Details { get; set; }
        
        [JsonPropertyName("location")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Location { get; set; }

        [JsonPropertyName("request_vector")]
        public float[]? RequestVector { get; set; }
    }
}
