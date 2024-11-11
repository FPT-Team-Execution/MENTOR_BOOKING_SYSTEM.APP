using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThirdPartyLayer.Services.Interfaces
{
    public interface ITemplateService
    {
        Task<string> GetTemplateAsync(string templateName);

        string ReplaceInTemplate(string input, IDictionary<string, string> replaceWords);
    }
}
