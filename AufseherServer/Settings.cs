namespace AufseherServer
{
	/// <summary>
	/// This service fetches all sorts of settings from the config files and environment variables.
	/// It throws ArgumentNullExceptions when it cannot find the required settings.
	/// </summary>
	/// <param name="configuration"></param>
	public class Settings(IConfiguration configuration)
	{
#if !DEBUG
		public string DiscordClientId = Environment.GetEnvironmentVariable("API_DISCORD_CLIENT_ID") ??
		                                throw new ArgumentNullException("API_DISCORD_CLIENT_ID");

		public string DiscordClientSecret = Environment.GetEnvironmentVariable("API_DISCORD_CLIENT_SECRET") ??
		                                    throw new ArgumentNullException("API_DISCORD_CLIENT_SECRET");

		public string DiscordBotToken = Environment.GetEnvironmentVariable("API_DISCORD_BOT_TOKEN") ??
		                                throw new ArgumentNullException("API_DISCORD_BOT_TOKEN");

		public string YouTubeToken = Environment.GetEnvironmentVariable("API_YOUTUBE_SECRET") ??
		                             throw new ArgumentNullException("API_YOUTUBE_SECRET");
#else
		public string DiscordClientId = configuration["Discord:Client:Id"] ??
		                                throw new ArgumentNullException("Discord:Client:Id");

		public string DiscordClientSecret = configuration["Discord:Client:Secret"] ??
		                                    throw new ArgumentNullException("Discord:Client:Secret");

		public string DiscordBotToken = configuration["Discord:Client:BotToken"] ??
		                                throw new ArgumentNullException("Discord:Client:BotToken");
		
		public string YouTubeToken = configuration["YouTube:Secret"] ??
		                             throw new ArgumentNullException("YouTube:Secret");

		public string UserHash = "1441b63c82b005285fc3b84cd69bd472eea279946b91764fe3637188a2e16b14";
		public string PwHash = "08e21d654eb82c97a8fc04a32f81b0fd408b55d9ef2a42a17475e880cce5d602"; 
#endif


	}
}