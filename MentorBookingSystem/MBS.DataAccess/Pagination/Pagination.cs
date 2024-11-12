namespace MBS.DataAccess.Pagination
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
