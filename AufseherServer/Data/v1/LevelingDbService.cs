using AufseherServer.Infrastructure.v1;
using AufseherServer.Models.v1;
using MongoDB.Driver;

namespace AufseherServer.Data.v1
{
	public class LevelingDbService: IService
	{
		private readonly IMongoCollection<LevelingModel> _collection;

		public LevelingDbService(MongoDbService mongoDbService, IConfiguration configuration)
		{
			IMongoDatabase? db = mongoDbService.GetDatabase(configuration["MongoDB:Database"]);
			_collection = db.GetCollection<LevelingModel>("Leveling");
		}

		public async Task<LevelingModel> GetLevelingAsync(ulong userId) =>
			await _collection.Find(x => x.UserId == userId).FirstOrDefaultAsync();
	}
}