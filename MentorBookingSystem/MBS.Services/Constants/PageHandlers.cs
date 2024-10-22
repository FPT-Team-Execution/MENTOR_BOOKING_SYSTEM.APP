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
    #endregion
}