using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;
using AufseherServer.Data.v1;
using AufseherServer.Infrastructure.v1;
using AufseherServer.Models.v1;
using AufseherServer.Services.v1.Interfaces;

namespace AufseherServer.Services.v1.Implementations
{
	public class AuthorizationService(IConfiguration configuration, Settings settings, AuthenticationDbService authDbService)
		: IAuthorizationService, IService
	{
		private readonly HttpClient _httpClient = new();
		
		private readonly string _redirectUri = configuration["Discord:Client:RedirectURI"] ??
			throw new ArgumentNullException();
		
		private readonly string _tokenUrl = configuration["Discord:OAuth2:TokenURL"] ??
			throw new ArgumentNullException();
		
		private readonly string _userUrl = configuration["Discord:OAuth2:UserURL"] ??
			throw new ArgumentNullException();
		
		
		public async Task<(int, string? ReasonPhrase)> HandleCallbackAsync(string code, string state)
		{
			ArgumentException.ThrowIfNullOrEmpty("code");

			var data = new Dictionary<string, string>
			{
				{ "client_id", settings.DiscordClientId },
				{ "client_secret", settings.DiscordClientSecret },
				{ "code", code },
				{ "grant_type", "authorization_code" },
				{ "redirect_uri", _redirectUri },
				{ "scope", "identify" }
			};

			var content = new FormUrlEncodedContent(data);
			HttpResponseMessage request =
				await _httpClient.PostAsync(_tokenUrl, content);
			if (!request.IsSuccessStatusCode)
			{
				return (400, request.ReasonPhrase);
			}

			var element = JsonSerializer.Deserialize<JsonObject>(request.Content.ReadAsStringAsync().Result)!;

			if (!element.ContainsKey("access_token"))
			{
				return (400, "No access token provided");
			}
			return (200, element["access_token"].ToString());
		}


		public async Task<string> GetUserDataAsync(string accessToken)
		{
			var request = new HttpRequestMessage(HttpMethod.Get, _userUrl);
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
			HttpResponseMessage response = await _httpClient.SendAsync(request);
			return await response.Content.ReadAsStringAsync();
		}

		public async Task<AuthenticationModel> GetUserByAccessCodeAsync(string code)
		{
			AuthenticationModel access = await authDbService.GetUserByAccessCodeAsync(code);
			return access;
		}
	}
}