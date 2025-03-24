namespace AufseherServer
{
	/// <summary>
	/// This service fetches all sorts of settings from the config files and environment variables.
	/// It throws ArgumentNullExceptions when it cannot find the required settings.
	/// </summary>
	/// <param name="configuration"></param>
	public class Settings(IConfiguration configuration)
	{

		public string DiscordClientId = configuration["Discord:Client:Id"] ??
		                                throw new ArgumentNullException("Discord:Client:Id");

		public string DiscordClientSecret = configuration["Discord:Client:Secret"] ??
		                                    throw new ArgumentNullException("Discord:Client:Secret");

		public string DiscordBotToken = configuration["Discord:Client:BotToken"] ??
		                                throw new ArgumentNullException("Discord:Client:BotToken");

		public string YouTubeToken = configuration["YouTube:Secret"] ??
		                             throw new ArgumentNullException("YouTube:Secret");

		public string UserHash = configuration["Login:UserHash"] ??
		                         throw new ArgumentNullException("Login:UserHash");
		
		public string PwHash = configuration["Login:PwHash"] ??
		                             throw new ArgumentNullException("Login:PwHash");
		
		
	}
}