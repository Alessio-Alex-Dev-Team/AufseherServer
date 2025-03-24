namespace AufseherServer.Infrastructure
{
	public static class SwaggerAuthorizeExtensions
	{
		public static IApplicationBuilder UseSwaggerAuthorization(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<SwaggerBasicAuthMiddleware>();
		}
	}
}