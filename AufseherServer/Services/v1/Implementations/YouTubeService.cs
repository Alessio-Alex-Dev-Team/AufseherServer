using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using AufseherServer.Infrastructure.v1;
using AufseherServer.Services.v1.Interfaces;

namespace AufseherServer.Services.v1.Implementations
{
	public class YouTubeService(Settings settings, IConfiguration configuration) : IYouTubeService, IService
	{
		public async Task SendNotificationAsync(string uri)
		{
			string channelUri = configuration["YouTube:ChannelUri"]
			                    ?? throw new ArgumentNullException("YouTube:ChannelUri is not set in appsettings.json");

			using var client = new HttpClient();
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bot", settings.DiscordBotToken);

			var content = new StringContent(JsonSerializer.Serialize(new
			{
				content = $"YouTube Push Notification {uri}"
			}), Encoding.UTF8, "application/json");

			await client.PostAsync(channelUri, content);
		}
	}
}