using Mapster;
using MBS.DataAccess.Pagination;
using MBS.Razor.Pages.AdminPage.MentorPage.Models;
using MBS.Services.Constants;
using MBS.Services.Dtos;
using MBS.Services.Models;
using MBS.Services.Models.Requests.Degree;
using MBS.Services.Models.Requests.Major;
using MBS.Services.Models.Requests.Mentor;
using MBS.Services.Models.Responses.Degree;
using MBS.Services.Models.Responses.Major;
using MBS.Services.Models.Responses.Mentor;
using MBS.Services.Services.Interfaces;
using MBS.Services.Utils;
using MBS.Services.Utils.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MBS.Razor.Pages.AdminPage.MentorPage;

public class Index : BaseAdminPage
{
    private IMentorService _mentorService;
    private IMajorService _majorService;
    public Pagination<MentorDto>? MentorPagination { get; set; } = new();
    public MentorDto? ChosenMentor { get; set; } = new();

    public string SortOrder { get; set; } = "asc";
    public string SearchName { get; set; } = string.Empty;

    public int Size { get; set; } = 5;
    public int PageIndex { get; set; } = 1;

    public Index(IMentorService mentorService, IMajorService majorService)
    {
        _mentorService = mentorService;
        _majorService = majorService;
    }

    private async Task LoadMentors()
    {
        var response = await _mentorService.GetMentorsAsync(PageIndex, Size);
        MentorPagination = response;

        SaveTempData(TempDataKeys.AdminKeys.MentorPagination, MentorPagination);
        SaveTempData(TempDataKeys.PageIndex, PageIndex);
        SaveTempData(TempDataKeys.PageSize, Size);
        SaveTempData(TempDataKeys.SortOrder, SortOrder);
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            await LoadMentors();
        }
        catch
        {
            SaveTempData(TempDataKeys.ErrorMessage, "Some error occurred");
            return RedirectToPage(RouteEndpoints.AdminStudent);
        }

        return Page();
    }

    public async Task<ActionResult> OnGetShowMentorDetail(string mentorId)
    {
        try
        {
            MentorPagination = GetTempData<Pagination<MentorDto>>(TempDataKeys.AdminKeys.MentorPagination)!;
            ChosenMentor = MentorPagination.Items.First(x => x.Id == mentorId);

            var degrees = await _mentorService.GetMentorDegrees(
                mentorId,
                1,
                100
            );

            var majors = await _majorService.GetMentorMajorsAsync(mentorId, 1, 100);

            ChosenMentor.Majors = majors.Items.Adapt<IEnumerable<MajorDto>>();
            ChosenMentor.Degrees = degrees.Items.Adapt<IEnumerable<DegreeDto>>();

            SaveTempData(TempDataKeys.AdminKeys.ChosenMentor, ChosenMentor);
            if (ChosenMentor == null)
                SaveTempDataString(TempDataKeys.ErrorMessage, "Student not found");
            else
                ChosenMentor = ChosenMentor;
        }
        catch (Exception e)
        {
            SaveTempDataString(TempDataKeys.ErrorMessage, "Some error occurred");
            Redirect(RouteEndpoints.AdminStudent);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostSearch(string searchName, string sortOrder)
    {
        try
        {
            //Set Sort variables
            SearchName = searchName;
            SortOrder = sortOrder;
            //Get Page Size and Page Index (if exist)
            var pageSizeData = GetTempData<string>(TempDataKeys.PageSize);
            if (pageSizeData != null && int.TryParse(pageSizeData, out int pageSize))
                Size = pageSize;
            var pageIndexData = GetTempData<string>(TempDataKeys.PageIndex);
            if (pageIndexData != null && int.TryParse(pageIndexData, out int pageIndex))
                PageIndex = pageIndex;
            //Load data
            await LoadMentors();

            var query = MentorPagination.Items.AsQueryable();

            if (!string.IsNullOrEmpty(SearchName))
            {
                var words = searchName.Split(" ");
                //* All() => all condition true from words in order to return true for where
                query = query.Where(s => words.All(c => s.FullName.ToLower().Contains(c.ToString().ToLower())));
            }

            MentorPagination.Items = query.ToList();
            //* modify total pages based on item
            MentorPagination.PageSize = Size;
            MentorPagination.PageIndex = MentorPagination.TotalPages < PageIndex ? 1 : PageIndex;
            //Save temp data to next use
            SaveTempData(TempDataKeys.SortOrder, SortOrder);
            SaveTempData(TempDataKeys.SearchName, SearchName);
            SaveTempData(TempDataKeys.AdminKeys.MentorPagination, MentorPagination);
        }
        catch (Exception e)
        {
            SaveTempDataString(TempDataKeys.ErrorMessage, "Some error occurred");
            Redirect(RouteEndpoints.AdminMentor);
        }


        return Page();
    }

    public async Task<IActionResult> OnPostPageNavigate(string pageIndex, string size)
    {
        try
        {
            var mentorPagination = GetTempData<Pagination<MentorModel>>(TempDataKeys.AdminKeys.MentorPagination)!;
            //set pageIndex and page Size
            Size = int.Parse(size);
            //if total item from previous load * previous total pages is lower or equal then new size -> pageIndex = 1
            if ((mentorPagination.TotalItems * mentorPagination.TotalItems) <= Size)
                PageIndex = 1;
            else
                PageIndex = int.Parse(pageIndex);
            //Save temp data to next use
            SaveTempData(TempDataKeys.PageIndex, PageIndex);
            SaveTempData(TempDataKeys.PageSize, Size);
            //Load data pagination from api
            await LoadMentors();
        }
        catch (Exception e)
        {
            SaveTempDataString(TempDataKeys.ErrorMessage, "Some error occurred");
            Redirect(RouteEndpoints.AdminStudent);
        }

        return Page();
    }

    public async Task<IActionResult> OnPutUpdate(MentorModel mentor)
    {
        var mentorModelRequest = mentor.Adapt<UpdateMentorRequest>();
        var data = await _mentorService.UpdateMentorAsync(mentorModelRequest);
        if (!data.IsSuccess)
        {
            SaveTempDataString(TempDataKeys.ErrorMessage, data.Message);
            return await OnGetShowMentorDetail(mentor.Id);
        }

        //Load data
        await LoadMentors();
        SaveTempDataString(TempDataKeys.SuccessMessage, "Update Successful");
        SaveTempData(TempDataKeys.AdminKeys.ChosenMentor, mentor);
        return await OnGetShowMentorDetail(mentor.Id);
    }

    public async Task<IActionResult> OnPost(MentorModel chosenMentor, string action)
    {
        try
        {
            switch (action)
            {
                // case "create":
                //     return await OnPostCreate();
                case "update":
                    return await OnPutUpdate(chosenMentor);
                default:
                    return Page();
            }
        }
        catch (Exception e)
        {
            SaveTempDataString(TempDataKeys.ErrorMessage, "Some error occurred");
            return Redirect(RouteEndpoints.AdminMentor);
        }
    }
}