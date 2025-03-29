using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AufseherServer.Models.v1
{
	public class BlacklistModel
	{
		[BsonId] [JsonIgnore] 
		public ObjectId Id { get; set; }
		
		[BsonElement("user_id")] 
		public ulong UserId { get; set; } = 0;
		
		[BsonElement("reason")] 
		public string Reason { get; set; } = string.Empty;
	}
}