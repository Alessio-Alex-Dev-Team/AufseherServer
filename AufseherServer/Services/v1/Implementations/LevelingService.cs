using AufseherServer.Infrastructure.v1;
using AufseherServer.Models.v1;
using AufseherServer.Services.v1.Interfaces;
using MongoDB.Driver;

namespace AufseherServer.Services.v1.Implementations
{
	public class LevelingService(MongoDbService mongoDbService, IConfiguration configuration)
		: ILevelingService, IService
	{
		private readonly IMongoCollection<LevelingModel> _levelingCollection =
			mongoDbService.GetDatabase(configuration["MongoDB:Database"]).GetCollection<LevelingModel>("Leveling");

		public async Task<LevelingModel> GetUserAsync(ulong userId) =>
			await _levelingCollection.Find(x => x.UserId == userId).FirstOrDefaultAsync();
	}
}