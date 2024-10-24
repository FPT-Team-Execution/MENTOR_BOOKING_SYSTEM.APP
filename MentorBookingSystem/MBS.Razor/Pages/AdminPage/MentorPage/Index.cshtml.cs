using MBS.Services.Models;
using MBS.Services.Models.Responses.Mentor;
using MBS.Services.Services.Interfaces;
using MBS.Services.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MBS.Razor.Pages.AdminPage.MentorPage;

public class Index : PageModel
{
    private IMentorService _mentorService;

    public Index(IMentorService mentorService)
    {
        _mentorService = mentorService;
    }

    public Pagination<MentorResponse>? MentorData { get; set; }
    public MentorResponse? SelectedMentor { get; set; }

    public async Task<ActionResult> OnGet(int page = 1, int size = 10)
    {
        var response = await _mentorService.GetMentorsAsync(page, size) as BaseModel<Pagination<MentorResponse>>;
        if (response.StatusCode != 200)
        {
            TempData["ErrorMessage"] = response.Message;
            return Page();
        }

        MentorData = response.ResponseRequestModel;
        return Page();
    }

    public async Task<ActionResult> OnGetShowMentorDetails(string mentorId)
    {
        try
        {
            var response = await _mentorService.GetMentorsAsync(1, 100) as BaseModel<Pagination<MentorResponse>>;
            if (response.StatusCode != 200)
            {
                TempData["ErrorMessage"] = response.Message;
                return Page();
            }

            MentorData = response.ResponseRequestModel;

            SelectedMentor = MentorData?.Items.FirstOrDefault(x => x.Id == mentorId);
            return Page();
        }
        catch (Exception e)
        {
            TempData["ErrorMessage"] = e.Message;
            return Page();
        }
    }
}