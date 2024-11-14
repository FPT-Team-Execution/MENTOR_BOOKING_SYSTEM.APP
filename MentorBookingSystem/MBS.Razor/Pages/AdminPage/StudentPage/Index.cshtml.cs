using System.Transactions;
using MBS.BusinessObject.Entities;
using MBS.Externals.Utils;
using MBS.DataAccess.Pagination;
﻿using System.Transactions;
using Mapster;
using MBS.BusinessObject.Entities;
using MBS.Externals.Utils;
using MBS.Services.Constants;
using MBS.Services.Constants.Enums;
using MBS.Services.Dtos;
using MBS.Services.Models;
using MBS.Services.Models.Requests.Student;
using MBS.Services.Models.Responses.Major;
using MBS.Services.Services.Interfaces;
using MBS.Services.Utils;
using MBS.Services.Utils.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace MBS.Razor.Pages.AdminPage.StudentPage;

public class Index : BaseAdminPage
{
    public Pagination<StudentDto> StudentPagination { get; set; } = new();
    public List<MajorDto> Majors { get; set; } = new();
    [BindProperty] public int NewPoint { get; set; } = 0;

    [BindProperty] public StudentDto ChosenStudent { get; set; } = new();

    public string SortOrder { get; set; } = "asc";
    public string SearchName { get; set; } = string.Empty;
    public int Size { get; set; } = 5;
    public int PageIndex { get; set; } = 1;


    private readonly IStudentService _studentService;
    private readonly IMajorService _majorService;
    private readonly IAuthService _authService;

    public Index(IStudentService studentService, IMajorService majorService, IAuthService authService)
    {
        _studentService = studentService;
        _majorService = majorService;
        _authService = authService;
    }

    private async Task LoadMajors()
    {
        var data = await _majorService.GetAllMajors();
        Majors = data.ToList();
        SaveTempData(TempDataKeys.AdminKeys.Majors, Majors);
    }

    private async Task LoadStudents()
    {
        var students = await _studentService.GetStudentsAsync(page: PageIndex, size: Size, SortOrder);
        StudentPagination = students;

        SaveTempData(TempDataKeys.AdminKeys.StudentPagination, StudentPagination);
        SaveTempData(TempDataKeys.PageIndex, PageIndex);
        SaveTempData(TempDataKeys.PageSize, Size);
        SaveTempData(TempDataKeys.SortOrder, SortOrder);
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            await LoadStudents();
            await LoadMajors();
        }
        catch
        {
            SaveTempData(TempDataKeys.ErrorMessage, "Some error occurred");
            return RedirectToPage(RouteEndpoints.AdminStudent);
        }

        return Page();
    }


    public async Task<IActionResult> OnGetShowStudentDetail(string studentId)
    {
        try
        {
            StudentPagination = GetTempData<Pagination<StudentDto>>(TempDataKeys.AdminKeys.StudentPagination)!;
            var chosenStudent = StudentPagination.Items.FirstOrDefault(x => x.UserId == studentId);
            SaveTempData(TempDataKeys.AdminKeys.ChosenStudent, chosenStudent);
            if (chosenStudent == null)
                SaveTempDataString(TempDataKeys.ErrorMessage, "Student not found");
            else
                ChosenStudent = chosenStudent;
        }
        catch (Exception)
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
            await LoadStudents();

            var query = StudentPagination.Items.AsQueryable();

            if (!string.IsNullOrEmpty(SearchName))
            {
                var words = searchName.Split(" ");
                //* All() => all condition true from words in order to return true for where
                query = query.Where(s => words.All(c => s.FullName.ToLower().Contains(c.ToString().ToLower())));
            }

            StudentPagination.Items = query.ToList();
            //* modify total pages based on item
            StudentPagination.PageSize = Size;
            StudentPagination.PageIndex = StudentPagination.TotalPages < PageIndex ? 1 : PageIndex;
            //Save temp data to next use
            SaveTempData(TempDataKeys.SortOrder, SortOrder);
            SaveTempData(TempDataKeys.SearchName, SearchName);
            SaveTempData(TempDataKeys.AdminKeys.StudentPagination, StudentPagination);
        }
        catch (Exception e)
        {
            SaveTempDataString(TempDataKeys.ErrorMessage, "Some error occurred");
            Redirect(RouteEndpoints.AdminStudent);
        }


        return Page();
    }

    public async Task<IActionResult> OnPostPageNavigate(string pageIndex, string size)
    {
        try
        {
            var studentPagination = GetTempData<Pagination<StudentDto>>(TempDataKeys.AdminKeys.StudentPagination)!;
            //set pageIndex and page Size
            Size = int.Parse(size);
            //if total item from previous load * previous total pages is lower or equal then new size -> pageIndex = 1
            if ((studentPagination.TotalItems * studentPagination.TotalItems) <= Size)
                PageIndex = 1;
            else
                PageIndex = int.Parse(pageIndex);
            //Save temp data to next use
            SaveTempData(TempDataKeys.PageIndex, PageIndex);
            SaveTempData(TempDataKeys.PageSize, Size);
            //Load data pagination from api
            await LoadStudents();
        }
        catch (Exception e)
        {
            SaveTempDataString(TempDataKeys.ErrorMessage, "Some error occurred");
            Redirect(RouteEndpoints.AdminStudent);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostCreate(StudentDto student)
    {
        using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            var user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                Email = student.Email,
                FullName = student.FullName,
                Gender = student.Gender,
                UserName = student.Email,
                EmailConfirmed = true,
                Birthday = student.Birthday,
                LockoutEnabled = student.LockoutEnabled
            };
            var randomPw = PasswordUtils.GenerateRandomPassword();
            var createUserResult = await _authService.CreateUserAsync(user, randomPw);

            if (!createUserResult)
            {
                TempData["ErrorMessage"] = "Register fail!";
                await LoadMajors();
                return Page();
            }


            // var newStudent = new StudentDto()
            // {
            //     MajorId = student.MajorId,
            //     UserId = user.Id,
            //     University = student.University,
            //     WalletPoint = 100,
            // };
            student.UserId = user.Id;
            var createStudentResult = await _studentService.CreateStudentAsync(student);

            if (string.IsNullOrEmpty(createStudentResult))
            {
                TempData["ErrorMessage"] = "Register fail!";
                await LoadMajors();
                return Page();
            }

            var addToRoleResult = await _authService.AddToRoleAsync(user, UserRole.Student);

            await _authService.SendVerifyEmail(user);

            transactionScope.Complete();

            if (!addToRoleResult)
            {
                TempData["ErrorMessage"] = "Register fail!";
                await LoadMajors();
                return Page();
            }

           
        }
        TempData["SuccessMessage"] = "Register successfully";
        await LoadStudents();
        return Page();
    }

    public async Task<IActionResult> OnPutUpdate(StudentDto student)
    {
        var updateResult = await _studentService.UpdateStudentAsync(student);
        if (!updateResult)
        {
            SaveTempDataString(TempDataKeys.ErrorMessage, "Update failed");
            return await OnGetShowStudentDetail(student.UserId);
        }

        //Load data
        await LoadStudents();
        SaveTempDataString(TempDataKeys.SuccessMessage, "Update Successful");
        SaveTempData(TempDataKeys.AdminKeys.ChosenStudent, student);
        return await OnGetShowStudentDetail(student.UserId);
    }

    public async Task<IActionResult> OnPutDebitPoint(string studentId)
    {
        var student =
            GetTempData<Pagination<StudentDto>>(TempDataKeys.AdminKeys.StudentPagination)!.Items.FirstOrDefault(x =>
                x.UserId == studentId);
        if (student == null)
        {
            SaveTempDataString(TempDataKeys.ErrorMessage, "Student not found");
            return Page();
        }

        //debit point
        student.WalletPoint -= NewPoint;
        //reset new point
        NewPoint = 0;
        var studentModelRequest = student.Adapt<UpdateStudentRequest>();
        var updateResult = await _studentService.UpdateStudentAsync(student);
        if (!updateResult)
        {
            SaveTempDataString(TempDataKeys.ErrorMessage, "Update failed");
            return await OnGetShowStudentDetail(student.UserId);
        }

        //Load data
        await LoadStudents();
        SaveTempDataString(TempDataKeys.SuccessMessage, "Update Successful");
        SaveTempData(TempDataKeys.AdminKeys.ChosenStudent, student);
        return await OnGetShowStudentDetail(student.UserId);
    }

    public async Task<IActionResult> OnPutCreditPoint(String studentId)
    {
        var student =
            GetTempData<Pagination<StudentDto>>(TempDataKeys.AdminKeys.StudentPagination)!.Items.FirstOrDefault(x =>
                x.UserId == studentId);
        if (student == null)
        {
            SaveTempDataString(TempDataKeys.ErrorMessage, "Student not found");
            return Page();
        }

        //credit point
        student.WalletPoint += NewPoint;
        //reset point
        NewPoint = 0;
        var studentModelRequest = student.Adapt<UpdateStudentRequest>();
        var updateResult = await _studentService.UpdateStudentAsync(student);
        if (!updateResult)
        {
            SaveTempDataString(TempDataKeys.ErrorMessage, "Update failed");
            return await OnGetShowStudentDetail(student.UserId);
        }

        //Load data
        await LoadStudents();
        SaveTempDataString(TempDataKeys.SuccessMessage, "Update Successful");
        SaveTempData(TempDataKeys.AdminKeys.ChosenStudent, student);
        return await OnGetShowStudentDetail(student.UserId);
    }

    public async Task<IActionResult> OnPutCreditModify(string studentId)
    {
        var student =
            GetTempData<Pagination<StudentDto>>(TempDataKeys.AdminKeys.StudentPagination)!.Items.FirstOrDefault(x =>
                x.UserId == studentId);
        if (student == null)
        {
            SaveTempDataString(TempDataKeys.ErrorMessage, "Student not found");
            return Page();
        }

        //debit point
        student.WalletPoint = NewPoint;
        //reset point
        NewPoint = 0;
        var studentModelRequest = student.Adapt<UpdateStudentRequest>();
        var updateResult = await _studentService.UpdateStudentAsync(student);
        if (!updateResult)
        {
            SaveTempDataString(TempDataKeys.ErrorMessage, "Update failed");
            return await OnGetShowStudentDetail(student.UserId);
        }

        //Load data
        await LoadStudents();
        SaveTempDataString(TempDataKeys.SuccessMessage, "Update Successful");
        SaveTempData(TempDataKeys.AdminKeys.ChosenStudent, student);
        return await OnGetShowStudentDetail(student.UserId);
    }

    public async Task<IActionResult> OnPost(StudentDto chosenStudent, string action)
    {
        try
        {
            switch (action)
            {
                case "create":
                    return await OnPostCreate(chosenStudent);
                case "update":
                    return await OnPutUpdate(chosenStudent);
                case "point-debit":
                    return await OnPutDebitPoint(chosenStudent.UserId);
                case "point-credit":
                    return await OnPutCreditPoint(chosenStudent.UserId);
                case "point-modify":
                    return await OnPutCreditPoint(chosenStudent.UserId);
                default:
                    return Page();
            }
        }
        catch (Exception e)
        {
            SaveTempDataString(TempDataKeys.ErrorMessage, "Some error occurred");
            return Redirect(RouteEndpoints.AdminStudent);
        }
    }
}