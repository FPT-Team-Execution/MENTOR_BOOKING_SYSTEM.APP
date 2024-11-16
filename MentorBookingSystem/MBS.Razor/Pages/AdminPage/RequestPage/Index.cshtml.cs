using MBS.BusinessObject.Entities;
using MBS.BusinessObject.Enums;
using MBS.DataAccess.Pagination;
using MBS.Services.Constants;
using MBS.Services.Models.Responses.Requests;
using MBS.Services.Services.Interfaces;
using MBS.Services.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MBS.Razor.Pages.AdminPage.RequestPage;

public class Index : BaseAdminPage
{

    public Pagination<RequestResponse> RequestPagination { get; set; } = new();
    public string SortOrder { get; set; } = "asc";
    public string search { get; set; } = "";

    public int Size { get; set; } = 5;
    public int PageIndex { get; set; } = 1;

    private readonly IRequestService _requestService;

    public Index(IRequestService requestService)
    {
        _requestService = requestService;
    }
    //public void OnGet()
    //{

    //}

    private async Task LoadProject()
    {
        string userId = string.Empty;
        if (HttpContext.Request.Cookies.TryGetValue(CookieNames.UserId, out string? id)) userId = id;
        //Todo: get all request from service
        RequestPagination = await _requestService.GetAllRequestPagination(PageIndex, Size, SortOrder);

        SaveTempData(TempDataKeys.AdminKeys.RequestPagination, RequestPagination);
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
            return RedirectToPage(RouteEndpoints.Admin);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostPageNavigate(string pageIndex, string size)
    {
        try
        {
            var requestPagination = GetTempData<Pagination<Request>>(TempDataKeys.AdminKeys.RequestPagination)!;
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

    //public async Task<IActionResult> OnPostDeny(string userId)
    //{
    //    bool check = await _requestService.UpdateRequestStatus(Guid.Parse(userId), RequestStatusEnum.Rejected);
    //    LoadProject();
    //    return Redirect(RouteEndpoints.MentorRequest);
    //}
}