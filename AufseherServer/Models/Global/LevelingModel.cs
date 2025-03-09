using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AufseherServer.Models.Global;

public class LevelingModel
{
    [BsonId] private ObjectId Id { get; set; } = ObjectId.Empty;

    [BsonElement("id")] public ulong UserId { get; set; } = 0;

    [BsonElement("xp")] public int XP { get; set; } = 0;

    [BsonElement("message_xp")] public int MessageXP { get; set; } = 0;

    [BsonElement("message_count")] public int MessageCount { get; set; } = 0;

    [BsonElement("total_voice_time")] public int TotalVoiceTime { get; set; } = 0;

    [BsonElement("voice_join")] public int VoiceJoin { get; set; } = 0;

    [BsonElement("voice_leave")] public int VoiceLeave { get; set; } = 0;

    [BsonElement("voice_xp")] public int VoiceXP { get; set; } = 0;

    [BsonElement("event_xp")] public int EventXP { get; set; } = 0;

    [BsonElement("event_last_drop")] public DateTime EventLastDrop { get; set; } = DateTime.MinValue;

    [BsonElement("stage_xp")] public int StageXP { get; set; } = 0;

    [BsonElement("total_stage_time")] public int TotalStageTime { get; set; } = 0;

    [BsonElement("events")] public Dictionary<string, Dictionary<string, int>> Events { get; set; } = [];
}

