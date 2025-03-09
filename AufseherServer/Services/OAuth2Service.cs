using System;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using AufseherServer.Models;


namespace AufseherServer.Services;

public class OAuth2Service : IOAuth2Service, IService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    
    public OAuth2Service(IConfiguration configuration)
    {
        _configuration = configuration;
        _httpClient = new HttpClient();
    }
    
    public async Task<(int, string)> HandleCallbackAsync(string code, string state)
    {
        ArgumentNullException.ThrowIfNullOrEmpty("code");

        var data = new Dictionary<string, string>
        {
            { "client_id", _configuration["Discord:Client:Id"]! },
            { "client_secret", _configuration["Discord:Client:Secret"]! },
            { "code", code },
            { "grant_type", "authorization_code" },
            { "redirect_uri", _configuration["Discord:Client:RedirectURI"]! },
            { "scope", "identify" }
        };

        var content = new FormUrlEncodedContent(data);
        HttpResponseMessage request = await _httpClient.PostAsync(_configuration["Discord:OAuth2:TokenURL"], content);
        if (!request.IsSuccessStatusCode)
        {
            return (400, request.ReasonPhrase);
        }
        JsonObject element = JsonSerializer.Deserialize<JsonObject>(request.Content.ReadAsStringAsync().Result);
        
        return (200, element["access_token"].ToString());
        
    }


    public async Task<string> GetUserDataAsync(string accessToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, _configuration["Discord:OAuth2:UserURL"]);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        HttpResponseMessage response = await _httpClient.SendAsync(request);
        return await response.Content.ReadAsStringAsync();
    }
}