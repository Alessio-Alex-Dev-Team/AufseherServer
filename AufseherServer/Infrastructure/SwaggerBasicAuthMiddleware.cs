using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace AufseherServer.Infrastructure
{

	/// <summary>
	/// Original is from https://github.com/domaindrivendev/Swashbuckle.WebApi/issues/384#issuecomment-410117400
	/// </summary>
	public class SwaggerBasicAuthMiddleware
	{
		private readonly RequestDelegate next;
		private readonly Settings _settings;

		public SwaggerBasicAuthMiddleware(RequestDelegate next, Settings settings)
		{
			this.next = next;
			_settings = settings;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			//Make sure we are hitting the swagger path, and not doing it locally as it just gets annoying :-)
			if (context.Request.Path.StartsWithSegments("/swagger") /* && !IsLocalRequest(context) */)
			{
				string authHeader = context.Request.Headers["Authorization"];
				if (authHeader != null && authHeader.StartsWith("Basic "))
				{
					// Get the encoded username and password
					string? encodedUsernamePassword =
						authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1]?.Trim();

					// Decode from Base64 to string
					string decodedUsernamePassword =
						Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));

					// Split username and password
					string username = decodedUsernamePassword.Split(':', 2)[0];
					string password = decodedUsernamePassword.Split(':', 2)[1];

					// Check if login is correct
					if (IsAuthorized(username, password))
					{
						await next.Invoke(context);
						return;
					}
				}

				// Return authentication type (causes browser to show login dialog)
				context.Response.Headers["WWW-Authenticate"] = "Basic";

				// Return unauthorized
				context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
			}
			else
			{
				await next.Invoke(context);
			}
		}



		public static string ComputeSha256Hash(string rawData)
		{
			using (SHA256 sha256Hash = SHA256.Create())
			{
				byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

				StringBuilder builder = new StringBuilder();
				for (int i = 0; i < bytes.Length; i++)
				{
					builder.Append(bytes[i].ToString("x2"));
				}

				return builder.ToString();
			}
		}

		public bool IsAuthorized(string username, string password)
		{
			// Check that username and password are correct

			string usernameHash = ComputeSha256Hash(username);
			string pwHash = ComputeSha256Hash(password);


			return usernameHash == _settings.UserHash && pwHash == _settings.PwHash;
		}


		public bool IsLocalRequest(HttpContext context)
		{
			//Handle running using the Microsoft.AspNetCore.TestHost and the site being run entirely locally in memory without an actual TCP/IP connection
			if (context.Connection.RemoteIpAddress == null && context.Connection.LocalIpAddress == null)
			{
				return true;
			}

			if (context.Connection.RemoteIpAddress.Equals(context.Connection.LocalIpAddress))
			{
				return true;
			}

			return IPAddress.IsLoopback(context.Connection.RemoteIpAddress);
		}
	}
}