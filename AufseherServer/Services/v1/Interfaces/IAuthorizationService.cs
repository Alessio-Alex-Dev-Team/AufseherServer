using AufseherServer.Models.v1;

namespace AufseherServer.Services.v1.Interfaces
{
	/// <summary>
	/// Handle all sorts of authorization tasks.
	/// </summary>
	public interface IAuthorizationService
	{
		/// <summary>
		///     Handle the callback from the Discord API.
		/// </summary>
		/// <param name="code">Code provided by the API</param>
		/// <param name="state">State (this is to differentiate between web and mobile</param>
		/// <returns>HTTP Code and the message</returns>
		Task<(int, string? ReasonPhrase)> HandleCallbackAsync(string code, string state);

		/// <summary>
		///     Fetch user data from Discords API.
		/// </summary>
		/// <param name="accessToken">Required to authenticate with the API.</param>
		/// <returns>JSON String with user data</returns>
		Task<string> GetUserDataAsync(string accessToken);

		/// <summary>
		///     Get the authentication model for a user by their access code. This is to check app access.
		/// </summary>
		/// <param name="accessCode">The access code. This is requested by the user via the bot.</param>
		/// <returns><see cref="AuthenticationModel" /> filled with the data</returns>
		Task<AuthenticationModel> GetUserByAccessCodeAsync(string accessCode);
	}
}