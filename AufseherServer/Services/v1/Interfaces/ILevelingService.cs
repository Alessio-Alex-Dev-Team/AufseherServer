using AufseherServer.Models.v1;

namespace AufseherServer.Services.v1.Interfaces
{
	public interface ILevelingService
	{
		/// <summary>
		/// Get the user's leveling data.
		/// </summary>
		/// <param name="userId">Discord ID of the user.</param>
		/// <returns><see cref="LevelingModel"/> instnace containing user leveling data.</returns>
		Task<LevelingModel> GetUserAsync(ulong userId);
	}
}