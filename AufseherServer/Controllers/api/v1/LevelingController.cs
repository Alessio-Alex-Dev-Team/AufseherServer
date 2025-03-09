using AufseherServer.Services;
using AufseherServer.Services.MongoDB;
using Microsoft.AspNetCore.Mvc;

namespace AufseherServer.Controllers.api.v1;


[ApiController, Route("api/v1/leveling")]
public class LevelingController(ILevelingService levelingService) : ControllerBase
{
    [HttpGet, Route("stats/{userId}")]
    public async Task<IActionResult> GetUserAsync(ulong userId)
    {
        var leveling = await levelingService.GetUserAsync(userId);
        return Ok(leveling);
    }
}