using AufseherServer.Models.Global;

namespace AufseherServer.Services;

public interface ILevelingService
{
    Task<LevelingModel> GetUserAsync(ulong userId);
}