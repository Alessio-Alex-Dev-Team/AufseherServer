using AufseherServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace AufseherServer.Controllers.api.v1;

[ApiController, Route("api/v1/youtube")]
public class YouTube(IYouTubeService youtubeService) : ControllerBase
{
    [Route("notification"), HttpPost]
    public async Task<IActionResult> Notification([FromBody] string notification)
    {
        return Ok();
    }
    
}