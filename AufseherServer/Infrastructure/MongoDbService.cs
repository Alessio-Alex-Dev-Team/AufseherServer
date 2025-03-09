using AufseherServer.Services.MongoDB;
using MongoDB.Driver;


namespace AufseherServer.Services;

public class MongoDbService(string connectionString)
{
    private readonly IMongoClient _client = new MongoClient(connectionString);
    private readonly Dictionary<string, IMongoDatabase> _databases = new();

    public IMongoDatabase GetDatabase(string dbName)
    {
        if (!_databases.ContainsKey(dbName))
        {
            _databases[dbName] = _client.GetDatabase(dbName);
        }
        return _databases[dbName];
    }
    
}