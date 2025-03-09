namespace AufseherServer.Services;

public interface IOAuth2Service
{
    Task<(int, string)> HandleCallbackAsync(string code, string state);
    Task<string> GetUserDataAsync(string accessToken);
}