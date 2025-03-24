namespace AufseherServer.Services.v1.Interfaces
{
	public interface IYouTubeService
	{
		/// <summary>
		/// Send the notification to the Discord channel configured in appsettings.json.
		/// </summary>
		/// <param name="uri">The URI that should be specified in the message that is sent (usually a YouTube Video URI)</param>
		/// <returns></returns>
		Task SendNotificationAsync(string uri);
	}
}