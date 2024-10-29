namespace MBS.Services.Constants;

public class PageHandlers
{
    //route: https://domain.com/Login?handler=<hanlder name>
    #region Login
    public const string LoginWithGoogle = "loginWithGoogle";
    public const string Callback = "callback";

    #endregion
    
    #region Admin
    public const string ShowStudentDetails = "showStudentDetail";
    public const string ShowMentorDetails = "showMentorDetail";
    public const string ShowGroupDetails = "showGroupDetail";
    public const string ShowMajorDetails = "showMajorDetail";
    public const string ShowPositionDetails = "showMajorDetail";
    public const string ShowSkillDetails = "showSkillDetail";


    #endregion
}