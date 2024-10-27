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

        #region  Project Page

        public const string ProjectPagination = "MentorPagination";
        public const string ChosenProject = "ChosenMentor";

        #endregion


    }
}