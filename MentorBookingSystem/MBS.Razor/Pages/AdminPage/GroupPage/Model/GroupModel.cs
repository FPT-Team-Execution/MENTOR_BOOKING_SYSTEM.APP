namespace MBS.Razor.Pages.AdminPage.GroupPage.Model
{
    public class GroupModel
    {
        public Guid id { get; set; }
        public string Name { get; set; }
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }

        public string StudentId { get; set; }
        public string studentName { get; set; }

        public Guid PositionId { get; set; }
        public string PositionName { get; set; }
    }
}
