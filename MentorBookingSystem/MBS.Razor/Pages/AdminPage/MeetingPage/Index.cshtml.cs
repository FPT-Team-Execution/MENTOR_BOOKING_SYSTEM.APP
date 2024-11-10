using MBS.Razor.Pages.AdminPage.MentorPage.Models;
using MBS.Razor.Pages.AdminPage.ProjectPage.Models;
using MBS.Services.Constants;
using MBS.Services.Models.Responses.Project;
using MBS.Services.Models;
using MBS.Services.Services.Interfaces;
using MBS.Services.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MBS.Razor.Pages.AdminPage.MeetingPage.Model;
using MBS.Services.Models.Responses.Meeting;
using Mapster;

namespace MBS.Razor.Pages.AdminPage.MeetingPage;

public class Index : BaseAdminPage
{
    public Pagination<MeetingModel> MeetingPagination { get; set; } = new();
    [BindProperty] public ProjectModel ChosenMeeting { get; set; } = new();

    public string SortOrder { get; set; } = "asc";
    public string search { get; set; } = "";

    public int Size { get; set; } = 5;
    public int PageIndex { get; set; } = 1;


    private readonly IMeetingService _meetingService;

    public Index(IMeetingService meetingService)
    {
        _meetingService = meetingService;
    }

    private async Task LoadMeeting()
    {
        var response = await _meetingService.GetMeetingAsync(PageIndex, Size) as BaseModel<Pagination<MeetingResponse>>;
        var meetingList = response!.ResponseRequestModel.Adapt<Pagination<MeetingModel>>();
        MeetingPagination = meetingList;

        SaveTempData(TempDataKeys.AdminKeys.MeetingPagination, MeetingPagination);
        SaveTempData(TempDataKeys.PageIndex, PageIndex);
        SaveTempData(TempDataKeys.PageSize, Size);

    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            await LoadMeeting();
        }
        catch
        {
            SaveTempData(TempDataKeys.ErrorMessage, "Some error occurred");
            return RedirectToPage(RouteEndpoints.AdminProject);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostPageNavigate(string pageIndex, string size)
    {
        try
        {
            var meetingPagination = GetTempData<Pagination<MeetingModel>>(TempDataKeys.AdminKeys.MeetingPagination)!;
            //set pageIndex and page Size
            Size = int.Parse(size);
            //if total item from previous load * previous total pages is lower or equal then new size -> pageIndex = 1
            if ((meetingPagination.TotalItems * meetingPagination.TotalItems) <= Size)
                PageIndex = 1;
            else
                PageIndex = int.Parse(pageIndex);
            //Save temp data to next use
            SaveTempData(TempDataKeys.PageIndex, PageIndex);
            SaveTempData(TempDataKeys.PageSize, Size);
            //Load data pagination from api
            await LoadMeeting();
        }
        catch (Exception e)
        {
            SaveTempDataString(TempDataKeys.ErrorMessage, "Some error occurred");
            Redirect(RouteEndpoints.AdminStudent);
        }

        return Page();
    }
}