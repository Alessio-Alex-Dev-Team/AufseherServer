using AufseherServer.Models.v1;
using AufseherServer.Services.v1.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AufseherServer.Controllers.v1
{
	/// <summary>
	///     API Controller for the Leveling Database.
	/// </summary>
	/// <param name="levelingService"></param>
	[ApiController]
	[Route("api/v1/leveling")]
	public class LevelingController(ILevelingService levelingService) : ControllerBase
	{
		/// <summary>
		///     Return a <see cref="LevelingModel" />
		/// </summary>
		/// <param name="userId">The ID of the user we want to request the Leveling data from.</param>
		/// <returns>200 attached with Leveling model</returns>
		[HttpGet]
		[Route("stats/{userId}")]
		public async Task<ActionResult<LevelingModel>> GetUserAsync(ulong userId)
		{
			LevelingModel? leveling = await levelingService.GetUserAsync(userId);
			return Ok(leveling);
		}
	}
}