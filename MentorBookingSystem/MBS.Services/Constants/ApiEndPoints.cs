namespace MBS.Services.Constants;

public class ApiEndPoints
{
    private static string baseUrl = "https://localhost:7554/api";

    public static string LoginUrl = $"{baseUrl}/auth/sign-in";
    public static string LogoutWithGoogleUrl = $"{baseUrl}/auth/logout";
    
}