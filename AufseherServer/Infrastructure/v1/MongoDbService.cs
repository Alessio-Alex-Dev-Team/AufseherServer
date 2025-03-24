using MongoDB.Driver;

namespace AufseherServer.Infrastructure.v1
{
	public class MongoDbService
	{
		private readonly MongoClient _client;
		private readonly Dictionary<string, IMongoDatabase> _databases = new();

		public MongoDbService(string connectionString)
		{
			if (string.IsNullOrEmpty(connectionString))
			{
				throw new ArgumentNullException(nameof(connectionString));
			}

			_client = new MongoClient(connectionString);
		}

		public IMongoDatabase GetDatabase(string dbName)
		{
			if (string.IsNullOrEmpty(dbName))
			{
				throw new ArgumentNullException(nameof(dbName));
			}
			
			if (_databases.TryGetValue(dbName, out IMongoDatabase? value))
			{
				return value;
			}

			value = _client.GetDatabase(dbName);
			_databases[dbName] = value;

			return value;
		}
	}
} 