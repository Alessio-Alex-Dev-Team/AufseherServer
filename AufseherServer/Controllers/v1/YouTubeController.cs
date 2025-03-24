using System.Xml.Linq;
using AufseherServer.Services.v1.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AufseherServer.Controllers.v1
{
	[ApiController]
	[Route("api/v1/youtube/notify")]
	public class YouTubeController(IYouTubeService youtubeService, Settings settings) : ControllerBase
	{
		private readonly string _youtubeToken = settings.YouTubeToken;


        /// <summary>
        ///     Initial response when subscribing to the YouTube API.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
		public IActionResult InitialResponse()
		{
			var hubChallenge = (string?)Request.Query["hub.challenge"];
			if (!string.IsNullOrEmpty(hubChallenge))
			{
				return Content(hubChallenge, "text/plain");
			}

			return BadRequest("Missing hub.challenge");
		}


        /// <summary>
        ///     Processing the notification from the YouTube API.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
		public async Task<IActionResult> PushNotificationAsync()
		{
			if (Request.Headers.Authorization != _youtubeToken)
			{
				return Unauthorized();
			}

			using var reader = new StreamReader(Request.Body);

			string xmlContent = await reader.ReadToEndAsync();
			XDocument xml = XDocument.Parse(xmlContent);

			if (xml.Root == null)
			{
				return BadRequest("Missing root");
			}

			XElement? entry = xml.Root.Element("entry");

			if (entry == null)
			{
				return BadRequest("Missing entry");
			}

			string? publishedStr = entry.Descendants("published").FirstOrDefault()?.Value;
			string? updatedStr = entry.Descendants("updated").FirstOrDefault()?.Value;
			string? videoUri = entry.Descendants("link").FirstOrDefault()?.Value;

			if (publishedStr == null || updatedStr == null || videoUri == null)
			{
				return BadRequest("Missing published, updated, or video URI");
			}

			DateTime published = DateTime.Parse(publishedStr);
			DateTime updated = DateTime.Parse(updatedStr);

			if (published != updated)
			{
				return BadRequest();
			}

			await youtubeService.SendNotificationAsync(videoUri);
			return Ok();
		}
	}
}