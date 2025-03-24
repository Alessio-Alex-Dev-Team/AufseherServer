using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AufseherServer.Models.v1
{
	public class AuthenticationModel
	{
		[BsonId] [JsonIgnore] public ObjectId Id { get; set; }

		[BsonElement("user_id")]
		[JsonPropertyName("user_id")]
		public ulong UserId { get; set; } = 0;

		[BsonElement("token")]
		[JsonPropertyName("token")]
		public string Token { get; set; }

		[BsonElement("access_code")]
		[JsonPropertyName("access_code")]
		public string AccessCode { get; set; }
	}
}