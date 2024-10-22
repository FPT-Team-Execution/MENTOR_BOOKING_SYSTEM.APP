using System.Text.Json.Serialization;

namespace MBS.Services.Models.Responses.Auth.GoogleAuth;

public class GoogleTokenResponse
{
    [JsonPropertyName("access_token")]
    public string access_token { get; set; }
    [JsonPropertyName("refresh_token")]
    public string refresh_token { get; set; }
    [JsonPropertyName("expires_in")]
    public int expires_in { get; set; }
    [JsonPropertyName("token_type")]
    public string token_type { get; set; }
    [JsonPropertyName("scope")]
    public string scope { get; set; }
}