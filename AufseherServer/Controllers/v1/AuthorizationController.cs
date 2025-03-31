using System.Text.Json;
using System.Text.Json.Nodes;
using AufseherServer.Models.v1;
using AufseherServer.Services.v1.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace AufseherServer.Controllers.v1
{
	/// <summary>
	///     Handle OAuth2 logins for external applications using the Discord API.
	/// </summary>
	[ApiController]
	[Route("api/v1/auth")]
	public class AuthorizationController(IAuthorizationService authorizationService) : ControllerBase
	{
		/// <summary>
		///     Store the tokens for the users to log them in and avoid revealing Discord tokens.
		/// </summary>
		private static readonly Dictionary<string, JsonObject> TokenStore = new();

		/// <summary>
		///     Sign the user in using the OAuth2 API from Discord.
		///     When the user is logged in, on iOS, we'll return to the application which will later call /user-data to get user
		///     data.
		///     On Web, we'll return the user object directly, to avoid unnecessary API calls.
		/// </summary>
		/// <param name="code">
		///     The callback code that is provided by Discords API calling this endpoint using the OAuth2 URl
		///     specified in the developer portal
		/// </param>
		/// <param name="state">
		///     A required state, which we use to determine the platform the user is operating on. This can be
		///     "web" or "ios" as of now
		/// </param>
		/// <returns></returns>
		[Route("sign-in")]
		[HttpGet]
		public async Task<IActionResult> OAuth2CallbackAsync([FromQuery] string code, [FromQuery] string state)
		{
			string? token = (await authorizationService.HandleCallbackAsync(code, state)).Item2;

			// in case this is null, we'll forward Discords error message to the user.
			if (token == null)
			{
				return BadRequest();
			}

			string userData = await authorizationService.GetUserDataAsync(token);

			if (state != "ios")
			{
				return Ok(userData);
			}

			var newToken = Guid.NewGuid().ToString();
			// just convert this to a json object and store it in the token store.
			TokenStore.Add(newToken, JsonSerializer.Deserialize<JsonObject>(userData)!);
			return Redirect("aufseherapp://auth?token=" + newToken);
		}

		/// <summary>
		///     This endpoint is used to get the user data from the token store.
		///     It is made specifically for mobile platforms, as we redirect the user to the app after the login.
		/// </summary>
		/// <returns>The OAuth2 User object according to Discords documentation</returns>
		[Route("user-data")]
		[HttpGet]
		public IActionResult UserDataAsync()
		{
			if (!Request.Headers.TryGetValue("Authorization", out StringValues token))
			{
				return Unauthorized();
			}

			if (token.Count == 0)
			{
				return Unauthorized();
			}

			if (!TokenStore.TryGetValue(token!, out JsonObject? returnValue))
			{
				return Unauthorized();
			}

			return Ok(returnValue);
		}

		/// <summary>
		///     This endpoint is used to verify that a user is registered and permitted to access the app.
		/// </summary>
		/// <param name="accessCode">The access code the user was provided with.</param>
		/// <returns></returns>
		[Route("user-access")]
		[HttpGet]
		public async Task<ActionResult<AuthenticationModel>> CheckAccessAsync([FromQuery] string accessCode)
		{
			return StatusCode(403, "Happy April 1st!"); // isn't displayed anyway but its a joke
			// try
			// {
			// 	AuthenticationModel? access = await authorizationService.GetUserByAccessCodeAsync(accessCode);
			// 	if (access == null)
			// 	{
			// 		return Unauthorized();
			// 	}
			//
			// 	return Ok(access);
			// }
			// catch (InvalidOperationException ex)
			// {
			// 	return StatusCode(403, ex.Message);
			// }
		}
	}
}