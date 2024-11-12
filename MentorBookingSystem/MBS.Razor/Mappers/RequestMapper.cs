

using Mapster;
using MBS.BusinessObject.Entities;
using MBS.Externals.Utils;
using MBS.Services.Dtos;

namespace MBS.Razor.Mappers;

public class RequestMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Request, RequestDto>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Start, src => src.Start)
            .Map(dest => dest.End, src => src.End)
            .Map(dest => dest.MentorId, src => src.MentorId)
            .Map(dest => dest.MentorName, src => src.Mentor.User.FullName)
            .Map(dest => dest.CreaterId, src => src.CreaterId)
            .Map(dest => dest.CreaterName, src => src.Creater.User.FullName)
            .Map(dest => dest.Status, src => src.Status.ToString())
            .Map(dest => dest.CreatedOn, src => src.CreatedOn);

    }
}