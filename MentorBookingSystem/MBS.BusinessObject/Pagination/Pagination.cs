using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBS.BusinessObject.Pagination
{
    public class Pagination<T> where T : class
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        private int _totalPage;

        public int TotalPages
        {
            get; set;
        }

        public IEnumerable<T> Items { get; set; }
        public int TotalItems { get; set; }

    }
}
