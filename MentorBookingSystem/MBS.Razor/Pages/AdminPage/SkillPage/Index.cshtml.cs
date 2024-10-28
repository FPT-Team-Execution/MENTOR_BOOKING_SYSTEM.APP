using Mapster;
using MBS.Razor.Pages.AdminPage.SkillPage.Model;
using MBS.Services.Constants;
using MBS.Services.Models;
using MBS.Services.Models.Requests.Skill;
using MBS.Services.Models.Responses.Skill;
using MBS.Services.Services.Interfaces;
using MBS.Services.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MBS.Razor.Pages.AdminPage.SkillPage
{
    public class Index : BaseAdminPage
    {
        public Pagination<SkillModel> SkillPagination { get; set; } = new();
        [BindProperty] public SkillModel ChosenSkill { get; set; } = new();
        public string SearchName { get; set; } = string.Empty;
        public string SortOrder { get; set; } = "asc";
        public int Size { get; set; } = 5;
        public int PageIndex { get; set; } = 1;
        private readonly ISkillService _skillService;

        public Index(ISkillService skillService)
        {
            _skillService = skillService;
        }

        private async Task LoadSkills()
        {
            var data = await _skillService.GetSkillsAsync(PageIndex, Size) as BaseModel<Pagination<SkillResponseDTO>>;
            var skillModels = data.ResponseRequestModel.Adapt<Pagination<SkillModel>>();

            SkillPagination = skillModels;
            SaveTempData(TempDataKeys.AdminKeys.SkillPagination, SkillPagination);
            SaveTempData(TempDataKeys.SearchName, SearchName);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                await LoadSkills();
            }
            catch
            {
                SaveTempData(TempDataKeys.ErrorMessage, "Some error occurred");
                return RedirectToPage(RouteEndpoints.AdminSkill);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostSearch(string searchName)
        {
            try
            {
                SearchName = searchName;
                var query = SkillPagination.Items.AsQueryable();
                if (!string.IsNullOrEmpty(SearchName))
                {
                    var words = searchName.Split(" ");
                    query = query.Where(s => words.All(c => s.Name.ToLower().Contains(c.ToString().ToLower())));
                }
                SkillPagination.Items = query.ToList();
            }
            catch
            {
                SaveTempData(TempDataKeys.ErrorMessage, "Some error occurred");
                return RedirectToPage(RouteEndpoints.AdminSkill);
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
            var request = new CreateNewSkillRequestDTO
            {
                Name = ChosenSkill.Name,
                MentorId = ChosenSkill.MentorId,
            };
            var response = await _skillService.CreateNewSkillAsync(request) as BaseModel<SkillResponseDTO>;
            if (response != null && response.IsSuccess)
            {
                SaveTempData(TempDataKeys.SuccessMessage, "Skill created successfully.");
                return RedirectToPage("Index");
            }
            else
            {
                SaveTempData(TempDataKeys.ErrorMessage, response?.Message ?? "Failed to create skill.");
                return Page();
            }
        }

        public async Task<IActionResult> OnPostUpdate()
        {
            // Logic to update the chosen skill
            return RedirectToPage("/Success");
        }

        public async Task<IActionResult> OnPostDelete(Guid skillId)
        {
            // Logic to delete the skill
            return RedirectToPage("/Success");
        }

        public IActionResult OnPost(SkillModel chosenSkill, string action)
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
                return (IActionResult)OnPostDelete(chosenSkill.id);
            }
            return Page();
        }
    }
}
