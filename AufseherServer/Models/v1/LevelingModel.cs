using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AufseherServer.Models.v1
{
	public class LevelingModel
	{
		[BsonId] [JsonIgnore] private ObjectId Id { get; set; } = ObjectId.Empty;

		[BsonElement("id")]
		[JsonPropertyName("userId")]
		public ulong UserId { get; set; } = 0;

		[BsonElement("xp")]
		[JsonPropertyName("xp")]
		public int XP { get; set; } = 0;

		[BsonElement("message_xp")]
		[JsonPropertyName("messageXP")]
		public int MessageXP { get; set; } = 0;

		[BsonElement("message_count")]
		[JsonPropertyName("messageCount")]
		public int MessageCount { get; set; } = 0;

		[BsonElement("total_voice_time")]
		[JsonPropertyName("totalVoiceTime")]
		public int TotalVoiceTime { get; set; } = 0;

		[BsonElement("voice_join")]
		[JsonPropertyName("voiceJoin")]
		public int VoiceJoin { get; set; } = 0;

		[BsonElement("voice_leave")]
		[JsonPropertyName("voiceLeave")]
		public int VoiceLeave { get; set; } = 0;

		[BsonElement("voice_xp")]
		[JsonPropertyName("voiceXP")]
		public int VoiceXP { get; set; } = 0;

		[BsonElement("event_xp")]
		[JsonPropertyName("eventXP")]
		public int EventXP { get; set; } = 0;

		[BsonElement("event_last_drop")]
		[JsonPropertyName("eventLastDrop")]
		public DateTime EventLastDrop { get; set; } = DateTime.MinValue;

		[BsonElement("stage_xp")]
		[JsonPropertyName("stageXP")]
		public int StageXP { get; set; } = 0;

		[BsonElement("total_stage_time")]
		[JsonPropertyName("totalStageTime")]
		public int TotalStageTime { get; set; } = 0;

		[BsonElement("events")]
		[JsonPropertyName("events")]
		public Dictionary<string, Dictionary<string, int>> Events { get; set; } = [];
	}
}