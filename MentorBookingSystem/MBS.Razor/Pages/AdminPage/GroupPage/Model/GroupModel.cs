using System.ComponentModel.DataAnnotations.Schema;

namespace MBS.Razor.Pages.AdminPage.GroupPage.Model
{
    public class GroupModel
    {
        public Guid id {get; set;}
        public Guid ProjectId {get; set;}
        public string ProjectName { get; set; }
        public string StudentId { get; set;}
        public string StudentName { get; set; }
        public Guid PositionId { get; set;}
        public string PositionName { get; set; }
    }
}
