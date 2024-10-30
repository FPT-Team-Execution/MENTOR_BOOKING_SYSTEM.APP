namespace MBS.Services.Constants;

public class ApiEndPoints
{
    private static string baseUrl = "https://localhost:7554/api";

    public static string LoginUrl = $"{baseUrl}/auth/sign-in";
    public static string LoginWithGoogleUrl = $"{baseUrl}/auth/signin-google";

    public static string RegisterUrl = $"{baseUrl}/auth/sign-up";

    public static string MajorUrl = $"{baseUrl}/majors";
    
    public static string StudentUrl = $"{baseUrl}/students";
    public static string StudentUpdateUrl = $"{StudentUrl}/profile";

    public static string MentorUrl = $"{baseUrl}/mentors";

    public static string GroupUrl = $"{baseUrl}/groups";

    public static string MajorsUrl = $"{baseUrl}/majors";
    public static string PositionsUrl = $"{baseUrl}/positions";
    public static string SkillUrl = $"{baseUrl}/skills";





}