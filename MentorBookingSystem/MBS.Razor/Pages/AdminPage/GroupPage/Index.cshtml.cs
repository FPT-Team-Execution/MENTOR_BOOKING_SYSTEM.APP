using Mapster;
using MBS.DataAccess.Pagination;
using MBS.Razor.Pages.AdminPage.GroupPage.Model;
using MBS.Services.Constants;
using MBS.Services.Models;
using MBS.Services.Models.Requests.Group;
using MBS.Services.Models.Responses.Group;
using MBS.Services.Services.Interfaces;
using MBS.Services.Utils;
using MBS.Services.Utils.Shared;
using Microsoft.AspNetCore.Mvc;

namespace MBS.Razor.Pages.AdminPage.GroupPage
{
    public class Index : BaseAdminPage
    {
        public Pagination<GroupModel> GroupPagination { get; set; } = new();
        [BindProperty] public GroupModel ChosenGroup { get; set; } = new();

        public string SearchName { get; set; } = string.Empty;
        public string SortOrder { get; set; } = "asc";

        public int Size { get; set; } = 5;
        public int PageIndex { get; set; } = 1;

        private readonly IGroupService _groupService;

        public Index(IGroupService groupService)
        {
            _groupService = groupService;
        }

        private async Task LoadGroups()
        {
            var data = await _groupService.GetGroupsAsync(PageIndex, Size) as BaseModel<Pagination<GroupResponse>>;
            var groupModels = data.ResponseRequestModel.Adapt<Pagination<GroupModel>>();
            GroupPagination = groupModels;
            SaveTempData(TempDataKeys.AdminKeys.GroupPagination, GroupPagination);
            SaveTempData(TempDataKeys.SearchName, SearchName);

        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                 await LoadGroups();
            }
            catch
            {
                SaveTempData(TempDataKeys.ErrorMessage, "Some error occurred");
                return RedirectToPage(RouteEndpoints.AdminGroup);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostSearch(string searchName)
        {
            try
            {
                SearchName = searchName;
                var query = GroupPagination.Items.AsQueryable();

                if (!string.IsNullOrEmpty(SearchName))
                {
                    var words = searchName.Split(" ");
                    query = query.Where(s => words.All(c => s.ProjectName.ToLower().Contains(c.ToString().ToLower())));
                }
                GroupPagination.Items = query.ToList();

            }
            catch
            {
                SaveTempData(TempDataKeys.ErrorMessage, "Some error occurred");
                return RedirectToPage(RouteEndpoints.AdminGroup);
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

            var request = new CreateNewGroupRequestModel
            {
                ProjectId = ChosenGroup.ProjectId,
                StudentId = ChosenGroup.StudentId,
                PositionId = ChosenGroup.PositionId
            };

            var response = await _groupService.CreateNewGroupAsync(request) as BaseModel<GroupResponse>;

            if (response != null && response.IsSuccess)
            {
                SaveTempData(TempDataKeys.SuccessMessage, "Group created successfully.");
                return RedirectToPage("Index"); // Hoặc trang bạn muốn chuyển đến sau khi tạo nhóm thành công
            }
            else
            {
                SaveTempData(TempDataKeys.ErrorMessage, response?.Message ?? "Failed to create group.");
                return Page();
            }
        }

        public async Task<IActionResult> OnPostUpdate()
        {
            // Logic to update the chosen group
            // Update the group using _groupService
            return RedirectToPage("/Success");
        }

        public async Task<IActionResult> OnPostDelete(Guid groupId)
        {
            // Logic to delete the group
            // Delete the group using _groupService
            return RedirectToPage("/Success");
        }

        public IActionResult OnPost(GroupModel chosenGroup, string action)
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
                return (IActionResult)OnPostDelete(chosenGroup.id);
            }

            return Page();
        }
    }
}
