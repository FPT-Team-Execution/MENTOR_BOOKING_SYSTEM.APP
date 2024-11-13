using MBS.BusinessObject.Entities;
using MBS.DataAccess.Pagination;
using MBS.Services.Constants;
using MBS.Services.Services.Interfaces;
using MBS.Services.Utils.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MBS.Razor.Pages.MentorPage.RequestPage;

public class Index : BaseMentorPage
{
    public Pagination<Request> RequestPagination { get; set; } = new();

    public string SortOrder { get; set; } = "asc";
    public string search { get; set; } = "";

    public int Size { get; set; } = 5;
    public int PageIndex { get; set; } = 1;


    private readonly IRequestService _requestService;

    public Index(IRequestService requestService)
    {
        _requestService = requestService;
    }

    private async Task LoadProject()
    {
        //Todo: get request from service
        
        SaveTempData(TempDataKeys.AdminKeys.ProjectPagination, RequestPagination);
        SaveTempData(TempDataKeys.PageIndex, PageIndex);
        SaveTempData(TempDataKeys.PageSize, Size);

    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            await LoadProject();
        }
        catch
        {
            SaveTempData(TempDataKeys.ErrorMessage, "Some error occurred");
            return RedirectToPage(RouteEndpoints.Mentor);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostPageNavigate(string pageIndex, string size)
    {
        try
        {
            var requestPagination = GetTempData<Pagination<Request>>(TempDataKeys.AdminKeys.ProjectPagination)!;
            Size = int.Parse(size);
            if ((requestPagination.TotalItems * requestPagination.TotalItems) <= Size)
                PageIndex = 1;
            else
                PageIndex = int.Parse(pageIndex);
            //Save temp data to next use
            SaveTempData(TempDataKeys.PageIndex, PageIndex);
            SaveTempData(TempDataKeys.PageSize, Size);
            //Load data pagination from api
            await LoadProject();
        }
        catch (Exception e)
        {
            SaveTempDataString(TempDataKeys.ErrorMessage, "Some error occurred");
            Redirect(RouteEndpoints.MentorRequest);
        }

        return Page();
    }
}