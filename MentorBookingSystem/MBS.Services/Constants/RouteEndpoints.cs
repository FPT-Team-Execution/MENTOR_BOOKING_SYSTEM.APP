namespace MBS.Services.Constants;

public static class RouteEndpoints
{
    #region Auth Routes

    public const string Login = "/Login";
    public const string Logout = "/Logout";
    public const string Register = "/Register";
    public const string AccessDenied = "/DeniedPage";

    #endregion

    #region Admin Routes

    // Admin routes
    public const string Admin = "/AdminPage";
    public const string AdminDashboard = "/AdminPage/Dashboard";

    // Admin Mentor Routes
    public const string AdminMentor = "/AdminPage/MentorPage";

    // Admin Skill Routes
    public const string AdminSkill = "/AdminPage/SkillPage";

    // Admin Request Routes
    public const string AdminRequest = "/AdminPage/RequestPage";
    public const string AdminRequestCreate = "/AdminPage/RequestPage/Create";

    // Admin Project Routes
    public const string AdminProject = "/AdminPage/ProjectPage";

    // Admin Position Routes
    public const string AdminPosition = "/AdminPage/PositionPage";

    // Admin Transaction Routes
    public const string AdminTransaction = "/AdminPage/TransactionPage";

    // Admin Student Routes
    public const string AdminStudent = "/AdminPage/StudentPage";

    // Admin Meeting Routes
    public const string AdminMeeting = "/AdminPage/MeetingPage";
    public const string AdminMeetingMember = "/AdminPage/MeetingPage/MemberPage";
    public const string AdminMeetingFeedback = "/AdminPage/MeetingPage/FeedbackPage";

    // Admin Major Routes
    public const string AdminMajor = "/AdminPage/MajorPage";

    // Admin Group Routes
    public const string AdminGroup = "/AdminPage/GroupPage";

    // Admin Calendar Routes
    public const string AdminCalender = "/AdminPage/CalendarPage";
    public const string AdminCalenderEvent = "/AdminPage/CalendarPage/EventPage";

    #endregion

    #region MentorRoutes

    public const string Mentor = "/MentorPage";

    // Mentor Meeting Routes
    public const string MentorMeeting = "/MentorPage/MeetingPage";
    public const string MentorMeetingMember = "/MentorPage/MeetingPage/MemberPage";
    public const string MentorMeetingFeedback = "/MentorPage/MeetingPage/FeedbackPage";

    // Mentor Calendar Routes
    public const string MentorCalender = "/MentorPage/CalendarPage";
    public const string MentorCalenderEvent = "/MentorPage/CalendarPage/EventPage";

    // Mentor Request Routes
    public const string MentorRequest = "/MentorPage/RequestPage";

    // Mentor Project Routes
    public const string MentorProject = "/MentorPage/ProjectPage";

    // Mentor Group Routes
    public const string MentorGroup = "/MentorPage/GroupPage";

    #endregion

    #region StudentRoutes

    public const string Student = "/StudentPage";
    public const string StudentProject = "/StudentPage/ProjectPage";

    #endregion
}