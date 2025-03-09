using MongoDB.Driver;
using AufseherServer.Models.Global;

namespace AufseherServer.Services.MongoDB;

public class LevelingDBService
{
    private readonly IMongoCollection<LevelingModel> _collection;

    public LevelingDBService(MongoDbService mongoDbService)
    {
        var db = mongoDbService.GetDatabase("Aufseher-dev");
        _collection = db.GetCollection<LevelingModel>("Leveling");
    }
    
    public async Task<LevelingModel> GetLevelingAsync(ulong userId)
    {
        return await _collection.Find(x => x.UserId == userId).FirstOrDefaultAsync();
    }
}