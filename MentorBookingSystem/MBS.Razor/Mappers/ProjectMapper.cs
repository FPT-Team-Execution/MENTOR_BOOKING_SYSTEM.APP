
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using MBS.BusinessObject.Entities;
using MBS.Services.Dtos;

namespace MBS.Razor.Mappers
{
    public class ProjectMapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Project, ProjectDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Title, src => src.Title)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.DueDate, src => src.DueDate)
                .Map(dest => dest.Semester, src => src.Semester)
                .Map(dest => dest.CreatedBy, src => src.CreatedBy)
                .Map(dest => dest.MentorId, src => src.MentorId)
                .Map(dest => dest.MentorName, src => src.Mentor.User.UserName)
                .Map(dest => dest.Status, src => src.Status.ToString());
        }
    }
}
