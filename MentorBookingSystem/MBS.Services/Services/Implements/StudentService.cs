using Mapster;
using MBS.BusinessObject.Entities;
using MBS.Repositories.Interfaces;
using MBS.Services.Models;
using MBS.Services.Services.Interfaces;
using StudentDto = MBS.Services.Dtos.StudentDto;

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

    public async Task<bool> UpdateStudentAsync(StudentDto student)
    {
        var studentFound = await _studentRepository.GetStudentByIdAsync(student.UserId);
        if (studentFound == null) return false;
        studentFound.User.FullName = student.FullName;
        studentFound.User.Email = student.Email;
        studentFound.University = student.University;
        studentFound.MajorId = student.MajorId;
        studentFound.User.Birthday = student.Birthday;
        studentFound.User.LockoutEnabled = student.LockoutEnabled;
        studentFound.User.Gender = student.Gender;
        var result = _studentRepository.Update(studentFound);
        return result;
    }

    public async Task<string> CreateStudentAsync(StudentDto studentDto)
    {
        var student = studentDto.Adapt<Student>();
        await _studentRepository.CreateAsync(student);
        return student.UserId;
    }
}