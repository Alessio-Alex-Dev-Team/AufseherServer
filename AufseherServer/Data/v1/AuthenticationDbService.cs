using AufseherServer.Infrastructure.v1;
using AufseherServer.Models.v1;
using MongoDB.Driver;

namespace AufseherServer.Data.v1
{
	public class AuthenticationDbService : IService
	{
		private readonly IMongoCollection<AuthenticationModel> _collection;

		public AuthenticationDbService(MongoDbService mongoDbService, IConfiguration configuration)
		{
			IMongoDatabase? db = mongoDbService.GetDatabase(configuration["MongoDB:Database"]);
			_collection = db.GetCollection<AuthenticationModel>("AppAccess");
		}

        /// <summary>
        ///     Get the authenticatoin model for a user by their access code.
        ///     This will be returned to the requester. The provided token is required to access the API.
        /// </summary>
        /// <param name="accessCode"></param>
        /// <returns></returns>
        public async Task<AuthenticationModel> GetUserByAccessCodeAsync(string accessCode) =>
			await _collection
				.Find(entry => entry.AccessCode == accessCode)
				.FirstOrDefaultAsync();
	}
}