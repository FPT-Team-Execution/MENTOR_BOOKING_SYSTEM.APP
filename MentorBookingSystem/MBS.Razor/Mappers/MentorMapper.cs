
using Mapster;
using MBS.BusinessObject.Entities;
using MBS.Services.Dtos;

namespace MBS.Razor.Mappers;

public class MentorMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Mentor, MentorDto>()
            .Map(dest => dest.Id, src => src.UserId)
            .Map(dest => dest.FullName, src => src.User.FullName)
            .Map(dest => dest.Email, src => src.User.Email)
            .Map(dest => dest.Industry, src => src.Industry)
            .Map(dest => dest.ConsumePoint, src => src.ConsumePoint)
            .Map(dest => dest.AvatarUrl, src => src.User.AvatarUrl)
            .Map(dest => dest.Gender, src => src.User.Gender)
            .Map(dest => dest.UserName, src => src.User.UserName)
            .Map(dest => dest.PhoneNumber, src => src.User.PhoneNumber)
            .Map(dest => dest.EmailConfirmed, src => src.User.EmailConfirmed)
            .Map(dest => dest.LockoutEnd, src => src.User.LockoutEnd.HasValue ? src.User.LockoutEnd.Value.DateTime : (DateTime?)null)
            .Map(dest => dest.LockoutEnabled, src => src.User.LockoutEnabled)
            .Map(dest => dest.CreatedBy, src => src.User.CreatedBy)
            .Map(dest => dest.CreatedOn, src => src.User.CreatedOn)
            .Map(dest => dest.UpdatedBy, src => src.User.UpdatedBy)
            .Map(dest => dest.UpdatedOn, src => src.User.UpdatedOn)
            .Map(dest => dest.Birthday, src => src.User.Birthday);
    }
}