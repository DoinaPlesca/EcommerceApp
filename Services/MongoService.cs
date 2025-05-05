using EcommerceApp.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EcommerceApp.Services;

public class MongoService
{
    private readonly IMongoDatabase _db;

    public MongoService(IOptions<MongoSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _db = client.GetDatabase(settings.Value.Database);
    }

    public IMongoCollection<T> GetCollection<T>(string name) =>
        _db.GetCollection<T>(name);
}