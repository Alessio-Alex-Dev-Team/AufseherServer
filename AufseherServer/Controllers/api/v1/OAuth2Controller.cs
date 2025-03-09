using System.Text.Json;
using System.Text.Json.Nodes;
using AufseherServer.Models;
using AufseherServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace AufseherServer.Controllers.api.v1;


/// <summary>
/// Handle OAuth2 logins for external applications using the Discord API.
/// </summary>
[ApiController, Route("api/v1/oauth2")]
public class OAuth2Controller(IOAuth2Service oAuth2Service) : ControllerBase
{
    private static Dictionary<string, JsonObject> _tokenStore = new();
    [Route("sign-in"), HttpGet]
    public async Task<IActionResult> Callback([FromQuery] string code, [FromQuery] string state)
    {
        string token = (await oAuth2Service.HandleCallbackAsync(code, state)).Item2;
        
        var userData = await oAuth2Service.GetUserDataAsync(token);
        
        if (state == "ios")
        {
            string newToken = Guid.NewGuid().ToString();
            _tokenStore.Add(newToken, JsonSerializer.Deserialize<JsonObject>(userData));
            return Redirect("aufseherapp://auth?token=" + newToken);
        }
        else
        {
            return Ok(userData);
        }
    }
    
    [Route("user-data"), HttpGet]
    public async Task<IActionResult> UserData()
    {
        var token = Request.Headers["Authorization"];
        if (!_tokenStore.ContainsKey(token))
        {
            return Unauthorized();
        }
        var returnValue = _tokenStore[token];
        return Ok("{\"user_id\": " + returnValue["id"] + ", \"user_data\": " + returnValue + "}");
    }
}