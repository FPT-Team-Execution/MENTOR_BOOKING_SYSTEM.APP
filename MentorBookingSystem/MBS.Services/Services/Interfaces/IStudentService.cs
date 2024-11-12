using MBS.DataAccess.Pagination;
using MBS.Services.Models;
using MBS.Services.Models.Requests.Student;
using MBS.Services.Models.Responses.Student;
using StudentDto = MBS.Services.Dtos.StudentDto;

namespace MBS.Services.Services.Interfaces;

public interface IStudentService
{
    Task<Pagination<StudentDto>> GetStudentsAsync(int page, int size, string sortOrder);
    Task<bool> UpdateStudentAsync(StudentDto student);
    Task<string> CreateStudentAsync(StudentDto student);


}