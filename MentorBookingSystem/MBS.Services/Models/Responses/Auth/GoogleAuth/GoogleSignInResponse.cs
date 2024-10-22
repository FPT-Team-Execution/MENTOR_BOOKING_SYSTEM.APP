namespace MBS.Services.Models.Responses.Auth.GoogleAuth;

public class GoogleSignInResponse
{
    public JwtResponse jwtModel { get; set; }
    public GoogleTokenResponse googleToken { get; set; }
}