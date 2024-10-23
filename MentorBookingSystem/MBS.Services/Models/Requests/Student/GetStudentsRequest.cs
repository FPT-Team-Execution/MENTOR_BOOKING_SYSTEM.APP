namespace MBS.Services.Models.Requests.Student;

public class GetStudentsRequest
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;

}