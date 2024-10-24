using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace MBS.Razor.Pages.AdminPage;

public class BaseAdminPage : PageModel
{
    /// <summary>
    /// Get temp data and keep for next use
    /// </summary>
    /// <param name="key">key of saved temp data</param>
    /// <typeparam name="T">cast result</typeparam>
    /// <returns>return object in case success, null in case exception</returns>
    public T? GetTempData<T>(string key) where T : class
    {
        try
        {
            var data = JsonConvert.DeserializeObject<T>(TempData[key] as string);
            TempData.Keep(key);
            return data;
        }
        catch (Exception e)
        {
            return null;
        }
    }
    /// <summary>
    /// Save value to temp data as string
    /// </summary>
    /// <param name="key">key</param>
    /// <param name="data">object</param>
    public void SaveTempData(string key, object? data)
    {
        TempData[key] = JsonConvert.SerializeObject(data); 
    }
    /// <summary>
    /// Keep data of key list
    /// </summary>
    /// <param name="keys"></param>
    public void KeepTempData(params string[] keys)
    {
        foreach (var key in keys)
        {
            TempData.Keep(key);
        }
    }
    /// <summary>
    /// Keep All data
    /// </summary>
    public void KeepAllTempData()
    {
        TempData.Keep();
    }
}