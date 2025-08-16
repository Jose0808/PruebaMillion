using MongoDB.Driver;
using RealEstate.Infrastructure.Configuration;
using RealEstate.Infrastructure.Data.Entities;

namespace RealEstate.Infrastructure.Data;

public interface IMongoDbContext
{
    IMongoCollection<MongoProperty> Properties { get; }
}

public class MongoDbContext : IMongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(MongoDbSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        _database = client.GetDatabase(settings.DatabaseName);
    }

    public IMongoCollection<MongoProperty> Properties =>
        _database.GetCollection<MongoProperty>("Properties");
}