using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MBS.BusinessObject.Enums;

namespace MBS.Services.Models.Responses;

public class RequestResponse
{
    public string Title { get; set; }
    public DateTime Start {  get; set; }
    public DateTime End { get; set; }
    public string ProjectName { get; set; }
    public RequestStatusEnum Status { get; set; }
    
}