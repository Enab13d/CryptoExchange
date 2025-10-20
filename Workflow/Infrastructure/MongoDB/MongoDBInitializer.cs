using MongoDB.Driver;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

public class MongoDBInitializer
{
    private readonly IMongoDatabase _database;
    private readonly ILogger<MongoDBInitializer> _logger;

    public MongoDBInitializer(IMongoDatabase database, ILogger<MongoDBInitializer> logger)
    {
        _database = database;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        var collections = new[]
        {
            "workflows",
            "workflow-instances",
            "subscriptions",
            "events",
            "executors"
        };

        foreach (var collectionName in collections)
        {
            if (!await CollectionExistsAsync(collectionName))
            {
                _logger.LogInformation("Creating collection {CollectionName}...", collectionName);
                await _database.CreateCollectionAsync(collectionName);
                _logger.LogInformation("Collection {CollectionName} created successfully", collectionName);
            }
        }
    }

    private async Task<bool> CollectionExistsAsync(string collectionName)
    {
        var filter = new BsonDocument("name", collectionName);
        var collections = await _database.ListCollectionsAsync(new ListCollectionsOptions { Filter = filter });
        return await collections.AnyAsync();
    }
}