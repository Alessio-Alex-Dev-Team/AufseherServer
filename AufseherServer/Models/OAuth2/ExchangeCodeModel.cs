using System.Text.Json;
using System.Text.Json.Serialization;

namespace AufseherServer.Models;


public class ExchangeCodeModel
{
    [JsonPropertyName("client_id")] public string ClientId { get; set; }
    
    [JsonPropertyName("client_secret")] public string ClientSecret { get; set; }
    
    [JsonPropertyName("code")] public string Code { get; set; }
    
    [JsonPropertyName("redirect_uri")] public string RedirectUri { get; set; }
    
    [JsonPropertyName("grant_type")] public string GrantType { get; set; } = "authorization_code";
    
    [JsonPropertyName("scope")] public string Scope { get; set; } = "identify guilds";
}