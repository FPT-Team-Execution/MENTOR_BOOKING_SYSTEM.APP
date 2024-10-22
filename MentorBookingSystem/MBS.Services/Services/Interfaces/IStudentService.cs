using MBS.Services.Models;
using MBS.Services.Models.Responses.Student;

namespace MBS.Services.Services.Interfaces;

public interface IStudentService
{
    Task<Pagination<StudentResponse>> GetStudentsAsync(int page, int size);
}