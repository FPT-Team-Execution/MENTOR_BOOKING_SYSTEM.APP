using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MBS.Externals.Services.Interfaces;

namespace MBS.Externals.Services.Implements
{
    public class TemplateService : ITemplateService
    {
        private readonly string _templatesPath;

        public TemplateService()
        {
            var projectPath = Directory.GetParent(Directory.GetCurrentDirectory())!.FullName;
            var templateProject = Assembly.GetExecutingAssembly().GetName().Name;

            _templatesPath = Path.Combine(projectPath, templateProject, "Templates");
        }

        public async Task<string> GetTemplateAsync(string templateName)
        {
            using var reader = new StreamReader(Path.Combine(_templatesPath, templateName));

            return await reader.ReadToEndAsync();
        }

        public string ReplaceInTemplate(string input, IDictionary<string, string> replaceWords)
        {
            var response = input;

            foreach (var temp in replaceWords)
            {
                response = response.Replace(temp.Key, temp.Value);
            }

            return response;
        }
    }
}
