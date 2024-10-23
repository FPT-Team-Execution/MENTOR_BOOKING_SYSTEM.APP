using MBS.Services.Constants;
using MBS.Services.Models;
using MBS.Services.Models.Requests.Major;
using MBS.Services.Models.Responses.Student;
using MBS.Services.Services.Interfaces;
using MBS.Services.Utils;

namespace MBS.Services.Services.Implements;

public class StudentService : IStudentService
{
    public async Task<Pagination<StudentResponse>> GetStudentsAsync(int page, int size)
    {
        var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW5AZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiI4ZmE3YzdiYi1kYWE1LWE2NjAtYmYwMi04MjMwMWE1ZWIzMjciLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsIm5iZiI6MTcyOTU5ODc4OCwiZXhwIjoxNzI5NjAyMzg4LCJpc3MiOiJtYnMtYmFja2VuZCIsImF1ZCI6Im1icy13ZWJhcHAifQ.TMFMdWBdEVXjcnUVZwulRqh78p54xmZAWzk033o-H_M";
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
                { "size", size.ToString() }
            }
        );
        var response = WebUtils.HandleResponse<BaseModel<Pagination<StudentResponse>>>(result);
        return response.ResponseRequestModel;
    }
}