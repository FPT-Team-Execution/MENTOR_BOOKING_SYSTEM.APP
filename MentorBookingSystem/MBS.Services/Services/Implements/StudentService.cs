using Mapster;
using MBS.Repositories.Interfaces;
using MBS.Services.Constants;
using MBS.Services.Models;
using MBS.Services.Models.Requests.Student;
using MBS.Services.Models.Responses.Major;
using MBS.Services.Models.Responses.Student;
using MBS.Services.Services.Interfaces;
using MBS.Services.Utils;

namespace MBS.Services.Services.Implements;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;

    public StudentService(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }
    public async Task<Pagination<StudentDto>> GetStudentsAsync(int page, int size, string sortOrder = "asc")
    {
        var students = await _studentRepository.GetStudentsAsync(page, size, sortOrder);
        return students.Adapt<Pagination<StudentDto>>();
    }

    public async Task<BaseModel<UpdateStudentResponse>> UpdateStudentAsync(UpdateStudentRequest student)
    {
        var token = WebUtils.AccessToken;
        var result = await WebUtils.PutAsync
        (
            ApiEndPoints.StudentUpdateUrl,
            data: student,
            headers: new Dictionary<string, string>
            {
                { "Accept-Charset", "utf-8" },
                { "Authorization", $"Bearer {token}" }
            },
            token: token
        );
        var response = WebUtils.HandleResponse<BaseModel<UpdateStudentResponse>>(result);
        return response;
    }

    public async Task<BaseModel<CreateStudentResponse, CreateStudentRequest>> CreateStudentAsync(CreateStudentRequest student)
    {
        var token = WebUtils.AccessToken;
        var result = await WebUtils.PostAsync
        (
            ApiEndPoints.StudentCreateUrl,
            data: student,
            headers: new Dictionary<string, string>
            {
                { "Accept-Charset", "utf-8" },
                { "Authorization", $"Bearer {token}" }
            },
            token: token
        );
        var response = WebUtils.HandleResponse<BaseModel<CreateStudentResponse, CreateStudentRequest>>(result);
        return response;
    }
}