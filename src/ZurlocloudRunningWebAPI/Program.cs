using Azure.Identity;
using Microsoft.Azure.Cosmos;
using ZurlocloudRunningWebAPI.Agents;
using ZurlocloudRunningWebAPI.Entities;
using ZurlocloudRunningWebAPI.Plugins;
using ZurlocloudRunningWebAPI.Services;
using Microsoft.Data.SqlClient;
using Azure.AI.OpenAI;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.ChatCompletion;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables from .env file
Env.Load();

var aoai_endpoint = Environment.GetEnvironmentVariable("AzureOpenAI__Endpoint");
var aoai_deploymentName = Environment.GetEnvironmentVariable("AzureOpenAI__DeploymentName");
var aoai_apiKey = Environment.GetEnvironmentVariable("AzureOpenAI__ApiKey");
var aoai_embeddingDeploymentName = Environment.GetEnvironmentVariable("AzureOpenAI__EmbeddingDeploymentName");
var cosmosdb_connectionString = Environment.GetEnvironmentVariable("CosmosDB__ConnectionString");

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables()
    .Build();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Use dependency injection to inject services into the application.
builder.Services.AddSingleton<IDatabaseService, DatabaseService>();
// builder.Services.AddSingleton<IVectorizationService, VectorizationService>();
builder.Services.AddSingleton<InventoryCopilot, InventoryCopilot>();

// Create a single instance of the CosmosClient to be shared across the application.
builder.Services.AddSingleton<CosmosClient>((_) =>
{
    CosmosClient client = new(
        connectionString: builder.Configuration["CosmosDB:ConnectionString"]!
    );
    return client;
});


builder.Services.AddSingleton<Kernel>((_) =>
{
    Console.WriteLine("Endpoint: " + aoai_endpoint);
    IKernelBuilder kernelBuilder = Kernel.CreateBuilder();
    kernelBuilder.AddAzureOpenAIChatCompletion(
        deploymentName: aoai_deploymentName!,
        endpoint: aoai_endpoint!,
        apiKey: aoai_apiKey!
    );
#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    kernelBuilder.AddAzureOpenAITextEmbeddingGeneration(
         deploymentName: aoai_deploymentName!,
         endpoint:aoai_endpoint!,
         apiKey: aoai_apiKey!
     );
#pragma warning restore SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    kernelBuilder.Plugins.AddFromType<DatabaseService>();
    kernelBuilder.Plugins.AddFromType<InventoryRequestPlugin>("InventoryCopilot");
    kernelBuilder.Services.AddSingleton<CosmosClient>((_) =>
    {
        CosmosClient client = new(
            connectionString: cosmosdb_connectionString!
        );
        return client;
    });

    return kernelBuilder.Build();
});


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
/**** Endpoints ****/
// This endpoint serves as the default landing page for the API.
app.MapGet("/", async () => 
{
    return "Welcome to the Zurlocloud Running Web API!";
})
    .WithName("Index")
    .WithOpenApi();

app.MapGet("/Stores", async () => 
{

    var stores = await app.Services.GetRequiredService<IDatabaseService>().GetStores();
    return stores;
})
    .WithName("GetStores")
    .WithOpenApi();


app.MapGet("/Products", async () => 
{

    var products = await app.Services.GetRequiredService<IDatabaseService>().GetProducts();
    return products;
})
    .WithName("GetProducts")
    .WithOpenApi();


// Retrieve the products for a specific store.
app.MapGet("/Stores/{storeId}/Products/", async (int storeId) => 
{
    var products = await app.Services.GetRequiredService<IDatabaseService>().GetProductsForStore(storeId);
    return products;
})
    .WithName("GetProductsForStore")
    .WithOpenApi();

// app.MapGet("/MultiProductOrder", async () => 
// {
//     var orders = await app.Services.GetRequiredService<IDatabaseService>().GetOrderDetailsWithMultipleProducts();
//     return orders;
// })
//     .WithName("GetOrderDetailsWithMultipleProducts")
//     .WithOpenApi();

// // Retrieve the bookings for a specific hotel that are after a specified date.
// app.MapGet("/Stores/{storeId}/Orders/{min_date}", async (int storeId, DateTime min_date) => 
// {
//     var bookings = await app.Services.GetRequiredService<IDatabaseService>().GetOrdersByDate(storeId, min_date);
//     return bookings;
// })
//     .WithName("GetRecentOrdersForStore")
//     .WithOpenApi();


// This endpoint is used to send a message to the Azure OpenAI endpoint.
app.MapPost("/Chat", async Task<string> (HttpRequest request) =>
{
    var message = await Task.FromResult(request.Form["message"]);
    Console.WriteLine("Message: " + message.ToString());
    var kernel = app.Services.GetRequiredService<Kernel>();
    var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
    var executionSettings = new OpenAIPromptExecutionSettings
    {
        ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
    };
    var response = await chatCompletionService.GetChatMessageContentAsync(message.ToString(), executionSettings, kernel);
    return response?.Content!;
})
    .WithName("Chat")
    .WithOpenApi();

// This endpoint is used to vectorize a text string.
// We will use this to generate embeddings for the maintenance request text.
app.MapGet("/Vectorize", async (string text, [FromServices] IVectorizationService vectorizationService) =>
{
    var embeddings = await vectorizationService.GetEmbeddings(text);
    return embeddings;
})
    .WithName("Vectorize")
    .WithOpenApi();

// This endpoint is used to search for maintenance requests based on a vectorized query.
app.MapPost("/VectorSearch", async ([FromBody] float[] queryVector, [FromServices] IVectorizationService vectorizationService, int max_results = 0, double minimum_similarity_score = 0.8) =>
{
    // Exercise 3 Task 3 TODO #3: Insert code to call the ExecuteVectorSearch function on the Vectorization Service. Don't forget to remove the NotImplementedException.
    var results = await vectorizationService.ExecuteVectorSearch(queryVector, max_results, minimum_similarity_score);
    return results;
})
    .WithName("VectorSearch")
    .WithOpenApi();

// This endpoint is used to send a message to the Maintenance Copilot.
app.MapPost("/InventoryCopilotChat", async ([FromBody]string message, [FromServices] InventoryCopilot copilot) =>
{
    // Exercise 5 Task 2 TODO #10: Insert code to call the Chat function on the MaintenanceCopilot. Don't forget to remove the NotImplementedException.
    var response = await copilot.Chat(message);
    return response;
})
    .WithName("Copilot")
    .WithOpenApi();


app.Run();