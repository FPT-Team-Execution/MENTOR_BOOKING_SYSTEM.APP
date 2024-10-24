namespace MBS.Services.Models;

public class Pagination<T> where T : class  
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public IEnumerable<T> Items { get; set; }
    
    public int TotalItems
    {
        get => Items?.Count() ?? 0;  // Set based on the count of Items
    }
}