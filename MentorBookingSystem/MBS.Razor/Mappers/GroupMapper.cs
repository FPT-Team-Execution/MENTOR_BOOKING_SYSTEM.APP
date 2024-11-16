
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using MBS.BusinessObject.Entities;
using MBS.Razor.Pages.AdminPage.GroupPage.Model;
using MBS.Services.Dtos;

namespace MBS.Razor.Mappers
{
    public class GroupMapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Group, GroupDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.ProjectId, src => src.ProjectId)
                .Map(dest => dest.StudentId, src => src.StudentId)
                .Map(dest => dest.StudentName, src => src.Student.User.UserName)
                .Map(dest => dest.PositionId, src => src.PositionId)
                .Map(dest => dest.PositionName, src => src.Position.Name)
                .Map(dest => dest.WalletPoint, src => src.Student.WalletPoint);
        }
    }
}
