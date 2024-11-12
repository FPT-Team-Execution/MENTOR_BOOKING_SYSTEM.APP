using MBS.BusinessObject.Entities;
using MBS.Razor.Pages.AdminPage.ProjectPage.Models;
using MBS.Services.Models.Requests.Project;
using MBS.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MBS.Razor.Pages.AdminPage.ProjectPage
{
    public class CreateModel : PageModel
    {
        private readonly IProjectService _projectService;

        [BindProperty]
        public CreateProjectModel Project { get; set; }

        public CreateModel(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _projectService.CreateProjectAsync(Project);
            return RedirectToPage("Index"); // Redirect to an index or list page after creating
        }
    }
}
