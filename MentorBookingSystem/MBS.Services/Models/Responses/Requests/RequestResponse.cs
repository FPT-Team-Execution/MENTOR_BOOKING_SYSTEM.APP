using MBS.BusinessObject.Enums;

namespace MBS.Services.Models.Responses.Requests;

public class RequestResponse
{
    public Guid RequestId { get; set; }
    public string Title { get; set; }
    public DateTime Start {  get; set; }
    public DateTime End { get; set; }
    public string ProjectName { get; set; }
    public RequestStatusEnum Status { get; set; }
    
}