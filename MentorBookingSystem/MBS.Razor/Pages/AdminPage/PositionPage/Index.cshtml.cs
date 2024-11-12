using Mapster;
using MBS.Razor.Pages.AdminPage.PositionPage.Model;
using MBS.Services.Constants;
using MBS.Services.Models;
using MBS.Services.Models.Requests.Position;
using MBS.Services.Models.Responses.Position;
using MBS.Services.Services.Interfaces;
using MBS.Services.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MBS.Services.Utils.Shared;
using MBS.DataAccess.Pagination;

namespace MBS.Razor.Pages.AdminPage.PositionPage
{
    public class Index : BaseAdminPage
    {
        public Pagination<PositionModel> PositionPagination { get; set; } = new();
        [BindProperty] public PositionModel ChosenPosition { get; set; } = new();
        public string SearchName { get; set; } = string.Empty;
        public string SortOrder { get; set; } = "asc";
        public int Size { get; set; } = 5;
        public int PageIndex { get; set; } = 1;
        private readonly IPositionService _positionService;

        public Index(IPositionService positionService)
        {
            _positionService = positionService;
        }

        private async Task LoadPositions()
        {
            var data = await _positionService.GetPositionsAsync(PageIndex, Size) as BaseModel<Pagination<PositionResponseDTO>>;
            var positionModels = data.ResponseRequestModel.Adapt<Pagination<PositionModel>>();

            PositionPagination = positionModels;
            SaveTempData(TempDataKeys.AdminKeys.PositionPagination, PositionPagination);
            SaveTempData(TempDataKeys.SearchName, SearchName);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                await LoadPositions();
            }
            catch
            {
                SaveTempData(TempDataKeys.ErrorMessage, "Some error occurred");
                return RedirectToPage(RouteEndpoints.AdminPosition);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostSearch(string searchName)
        {
            try
            {
                SearchName = searchName;
                var query = PositionPagination.Items.AsQueryable();
                if (!string.IsNullOrEmpty(SearchName))
                {
                    var words = searchName.Split(" ");
                    query = query.Where(s => words.All(c => s.Name.ToLower().Contains(c.ToString().ToLower())));
                }
                PositionPagination.Items = query.ToList();
            }
            catch
            {
                SaveTempData(TempDataKeys.ErrorMessage, "Some error occurred");
                return RedirectToPage(RouteEndpoints.AdminPosition);
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
            var request = new CreateNewPositionRequestModel
            {
                Name = ChosenPosition.Name,
                Description = ChosenPosition.Description,
            };
            var response = await _positionService.CreateNewPositionAsync(request) as BaseModel<PositionResponseDTO>;
            if (response != null && response.IsSuccess)
            {
                SaveTempData(TempDataKeys.SuccessMessage, "Position created successfully.");
                return RedirectToPage("Index");
            }
            else
            {
                SaveTempData(TempDataKeys.ErrorMessage, response?.Message ?? "Failed to create position.");
                return Page();
            }
        }

        public async Task<IActionResult> OnPostUpdate()
        {
            // Logic to update the chosen position
            return RedirectToPage("/Success");
        }

        public async Task<IActionResult> OnPostDelete(Guid positionId)
        {
            // Logic to delete the position
            return RedirectToPage("/Success");
        }

        public IActionResult OnPost(PositionModel chosenPosition, string action)
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
                return (IActionResult)OnPostDelete(chosenPosition.Id);
            }
            return Page();
        }
    }
}
