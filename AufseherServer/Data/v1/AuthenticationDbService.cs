using AufseherServer.Infrastructure.v1;
using AufseherServer.Models.v1;
using MongoDB.Driver;

namespace AufseherServer.Data.v1
{
	public class AuthenticationDbService : IService
	{
		private readonly IMongoCollection<AuthenticationModel> _appAccessCollection;
		private readonly IMongoCollection<BlacklistModel> _blacklistCollection;

		public AuthenticationDbService(MongoDbService mongoDbService, IConfiguration configuration)
		{
			IMongoDatabase? db = mongoDbService.GetDatabase(configuration["MongoDB:GlobalDatabase"]);
			_appAccessCollection = db.GetCollection<AuthenticationModel>("AppAccess");
			_blacklistCollection = db.GetCollection<BlacklistModel>("Blacklist");
		}

        /// <summary>
        ///     Get the authentication model for a user by their access code.
        ///     This will be returned to the requester. The provided token is required to access the API.
        /// </summary>
        /// <param name="accessCode"></param>
        /// <returns></returns>
        public async Task<AuthenticationModel> GetUserByAccessCodeAsync(string accessCode) =>
			await _appAccessCollection
				.Find(entry => entry.AccessCode == accessCode)
				.FirstOrDefaultAsync();
        
        /// <summary>
        /// Someone who isn't blacklisted won't be shown here.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<BlacklistModel> GetBlacklistEntryAsync(ulong userId) =>
			await _blacklistCollection
				.Find(entry => entry.UserId == userId)
				.FirstOrDefaultAsync();
        
	}
}