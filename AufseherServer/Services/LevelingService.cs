using AufseherServer.Models.Global;
using MongoDB.Driver;

namespace AufseherServer.Services;

public class LevelingService(MongoDbService mongoDbService) : ILevelingService, IService
{
    private readonly IMongoCollection<LevelingModel> _levelingCollection =
        mongoDbService.GetDatabase("Aufseher-dev").GetCollection<LevelingModel>("Leveling");
    
    public async Task<LevelingModel> GetUserAsync(ulong userId)
    {
        return await _levelingCollection.Find(x => x.UserId == userId).FirstOrDefaultAsync();
    }
}