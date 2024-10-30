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
    public async Task<Pagination<StudentResponse>> GetStudentsAsync(int page, int size, string sortOrder = "asc")
    {
        var token = WebUtils.AccessToken;
        var result = await WebUtils.GetAsync
        (
            ApiEndPoints.StudentUrl,
            headers: new Dictionary<string, string>
            {
                { "Accept-Charset", "utf-8" },
                { "Authorization", $"Bearer {token}" }
            },
            token: token,
            queryParams: new Dictionary<string, string?>()
            {
                { "page", page.ToString() },
                { "size", size.ToString() },
                {"sortOrder", sortOrder }
            }
        );
        var response = WebUtils.HandleResponse<BaseModel<Pagination<StudentResponse>>>(result);
        return response.ResponseRequestModel;
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
}