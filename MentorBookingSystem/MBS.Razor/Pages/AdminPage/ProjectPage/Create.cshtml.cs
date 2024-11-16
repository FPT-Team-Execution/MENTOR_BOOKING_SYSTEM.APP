using MBS.Services.Constants;
using MBS.Services.Dtos;
using MBS.Services.Models.Requests.Project;
using MBS.Services.Models.Responses.Mentor;
using MBS.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MBS.Razor.Pages.AdminPage.ProjectPage
{
    public class CreateModel : PageModel
    {
        private readonly IProjectService _projectService;
        private readonly IMentorService _mentorService; // Add your MentorService here

        [BindProperty]
        public CreateProjectModel Project { get; set; }

        public List<MentorDto> Mentors { get; set; } // List to hold mentors

        public CreateModel(IProjectService projectService, IMentorService mentorService)
        {
            _projectService = projectService;
            _mentorService = mentorService;
        }

        public async Task OnGetAsync()
        {
            IEnumerable<MentorDto> mentors = await _mentorService.GetMentorsAsync();
            Mentors = mentors.ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _projectService.CreateProjectAsync(Project);
                return Redirect(RouteEndpoints.AdminProject);
            } catch (Exception e)
            {
                return Redirect(RouteEndpoints.AdminProject);
            }
        }
    }
}
