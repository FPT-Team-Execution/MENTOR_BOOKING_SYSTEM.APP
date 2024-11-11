using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Externals.Services.Interfaces
{
    public interface ITemplateService
    {
        Task<string> GetTemplateAsync(string templateName);

        string ReplaceInTemplate(string input, IDictionary<string, string> replaceWords);
    }
}
