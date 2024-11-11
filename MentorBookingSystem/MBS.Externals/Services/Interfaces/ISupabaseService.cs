using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Externals.Services.Interfaces
{
    public interface ISupabaseService
    {
        Task<string> UploadFile(byte[] fileByte, string filePath, string bucketName, bool replace);
        string RetrievePublicUrl(string bucketName, string filePath);
    }
}
