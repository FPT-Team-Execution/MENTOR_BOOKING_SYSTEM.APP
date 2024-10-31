using MBS.Services.Models;
using MBS.Services.Models.Requests.Student;
using MBS.Services.Models.Responses.Student;

namespace MBS.Services.Services.Interfaces;

public interface IStudentService
{
    Task<Pagination<StudentResponse>> GetStudentsAsync(int page, int size, string sortOrder);
    Task<BaseModel<UpdateStudentResponse>> UpdateStudentAsync(UpdateStudentRequest student);
    Task<BaseModel<CreateStudentResponse, CreateStudentRequest>> CreateStudentAsync(CreateStudentRequest student);


}