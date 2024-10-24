using MBS.Services.Models.Responses.Mentor;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MBS.Razor.Pages.AdminPage.MentorPage;

public class MentorDetailPartial : PageModel
{
    public MentorResponse SelectedMentor { get; set; }
    
    public void OnGet()
    {
        
    }
}