using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBS.Externals.Models.Email;

namespace MBS.Externals.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailMessage emailMessage);
    }
}
