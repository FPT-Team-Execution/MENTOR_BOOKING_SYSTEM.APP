namespace MBS.Services.Utils;

public class TempDataKeys
{
    //* Notification
    public const string SuccessMessage = "SuccessMessage";
    public const string ErrorMessage = "ErrorMessage";
    //* Pagination & Search & Sort 
    public const string SortOrder = "SortOrder";
    public const string SearchName = "SearchName";
    
    public const string PageIndex = "PageIndex";
    public const string PageSize = "PageSize";
    public class AdminKeys
    {
        #region Student Page

        public const string StudentPagination = "StudentPagination";
        public const string ChosenStudent = "ChosenStudent";
        public const string Majors = "Majors";
        
        #endregion

        #region  Mentor Page

        public const string MentorPagination = "MentorPagination";
        public const string ChosenMentor= "ChosenMentor";

        #endregion

        #region  Group Page

        public const string GroupPagination = "GroupPagination";
        public const string ChosenGroup = "ChosenGroup";

        #endregion

        #region  Major Page

        public const string MajorPagination = "MajorPagination";
        public const string ChosenMajor = "ChosenMajor";

        #endregion

        #region  Position Page

        public const string PositionPagination = "PositionPagination";
        public const string ChosenPosition = "ChosenPosition";

        #endregion

        #region  Skill Page

        public const string SkillPagination = "SkillPagination";
        public const string ChosenSkill = "ChosenSkill";

        #endregion

        #region Project Page

        public const string ProjectPagination = "ProjectPagination";
        public const string ChosenProject = "ChosenProject";

        #endregion

        #region Meeting Page

        public const string MeetingPagination = "MeetingPagination";
        public const string ChosenMeeting = "ChosenMeeting";

        #endregion


    }
}