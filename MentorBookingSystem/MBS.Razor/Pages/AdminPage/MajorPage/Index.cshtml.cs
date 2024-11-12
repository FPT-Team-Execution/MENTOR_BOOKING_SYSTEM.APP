using Mapster;
using MBS.Razor.Pages.AdminPage.MajorPage.Model;
using MBS.Services.Constants;
using MBS.Services.Models;
using MBS.Services.Models.Requests.Major;
using MBS.Services.Models.Responses.Major;
using MBS.Services.Services.Interfaces;
using MBS.Services.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using MBS.Services.Utils.Shared;
using MBS.DataAccess.Pagination;

namespace MBS.Razor.Pages.AdminPage.MajorPage
{
    public class Index : BaseAdminPage
    {
        public Pagination<MajorModel> MajorPagination { get; set; } = new();
        [BindProperty] public MajorModel ChosenMajor { get; set; } = new();
        public string SearchName { get; set; } = string.Empty;
        public string SortOrder { get; set; } = "asc";
        public int Size { get; set; } = 5;
        public int PageIndex { get; set; } = 1;
        private readonly IMajorService _majorService;

        public Index(IMajorService majorService)
        {
            _majorService = majorService;
        }

        private async Task LoadMajors()
        {
            var data = await _majorService.GetMajorsAsync(PageIndex, Size) as BaseModel<Pagination<MajorResponseDto>>;
            var majorModels = data.ResponseRequestModel.Adapt<Pagination<MajorModel>>();

            MajorPagination = majorModels;
            SaveTempData(TempDataKeys.AdminKeys.MajorPagination, MajorPagination);
            SaveTempData(TempDataKeys.SearchName, SearchName);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                await LoadMajors();
            }
            catch
            {
                SaveTempData(TempDataKeys.ErrorMessage, "Some error occurred");
                return RedirectToPage(RouteEndpoints.AdminMajor);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostSearch(string searchName)
        {
            try
            {
                SearchName = searchName;
                var query = MajorPagination.Items.AsQueryable();
                if (!string.IsNullOrEmpty(SearchName))
                {
                    var words = searchName.Split(" ");
                    query = query.Where(s => words.All(c => s.MajorName.ToLower().Contains(c.ToString().ToLower())));
                }
                MajorPagination.Items = query.ToList();
            }
            catch
            {
                SaveTempData(TempDataKeys.ErrorMessage, "Some error occurred");
                return RedirectToPage(RouteEndpoints.AdminMajor);
            }
            return RedirectToPage("Index");
        }

        public async Task<IActionResult> OnPostCreate()
        {
            if (!ModelState.IsValid)
            {
                SaveTempData(TempDataKeys.ErrorMessage, "Please check the input data.");
                return Page();
            }
            var request = new CreateNewMajorRequestModel
            {
                MajorName = ChosenMajor.MajorName,
                ParentId = ChosenMajor.ParentId,

            };
            var response = await _majorService.CreateNewMajorAsync(request) as BaseModel<MajorResponseDto>;
            if (response != null && response.IsSuccess)
            {
                SaveTempData(TempDataKeys.SuccessMessage, "Major created successfully.");
                return RedirectToPage("Index");
            }
            else
            {
                SaveTempData(TempDataKeys.ErrorMessage, response?.Message ?? "Failed to create major.");
                return Page();
            }
        }

        public async Task<IActionResult> OnPostUpdate()
        {
            // Logic to update the chosen major
            // Update the major using _majorService
            return RedirectToPage("/Success");
        }

        public async Task<IActionResult> OnPostDelete(Guid majorId)
        {
            // Logic to delete the major
            // Delete the major using _majorService
            return RedirectToPage("/Success");
        }

        public IActionResult OnPost(MajorModel chosenMajor, string action)
        {
            if (action == "create")
            {
                return (IActionResult)OnPostCreate();
            }
            if (action == "update")
            {
                return (IActionResult)OnPostUpdate();
            }
            if (action == "delete")
            {
                return (IActionResult)OnPostDelete(chosenMajor.Id);
            }
            return Page();
        }
    }
}
