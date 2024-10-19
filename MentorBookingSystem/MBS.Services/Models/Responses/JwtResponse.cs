using System.Text.Json.Serialization;

namespace MBS.Services.Models.Responses;


public class JwtResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}