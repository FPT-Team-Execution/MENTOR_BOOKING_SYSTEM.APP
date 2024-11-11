using Microsoft.AspNetCore.Mvc;

namespace MBS.Services.Services.Interfaces;

public interface IFileService<T> where T : class
{
    public IEnumerable<T> Import(string filePath, string sheetName);
    
    public Task<byte[]> Export(IEnumerable<T> data);

}