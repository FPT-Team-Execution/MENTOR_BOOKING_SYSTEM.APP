using System.Text.Json.Serialization;

namespace MBS.Services.Models.Responses.Auth;

public class LoginResponse
{
    public JwtResponse JwtToken { get; set; }
}