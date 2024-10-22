using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.Services.Models
{
    public class BaseModel<TResponseModel, TRequestModel> : IResponse where TResponseModel : class where TRequestModel : class
    {
        public bool IsSuccess { get; set; }
        public required string Message { get; set; }
        public int StatusCode { get; set; }
        public TResponseModel? ResponseModel { get; set; }
        public TRequestModel? RequestModel { get; set; }
    }

    public class BaseModel<TResponseRequestModel> : IResponse where TResponseRequestModel : class
    {
        public bool IsSuccess { get; set; }
        public required string Message { get; set; }
        public int StatusCode { get; set; }
        public required TResponseRequestModel ResponseRequestModel { get; set; }
    };

    public class BaseModel : IResponse
    {
        public bool IsSuccess { get; set; }
        public required string Message { get; set; }
        public int StatusCode { get; set; }
    };
    
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
}
