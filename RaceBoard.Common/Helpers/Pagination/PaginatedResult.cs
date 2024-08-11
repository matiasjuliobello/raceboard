using RaceBoard.Common.Helpers.Interfaces;

namespace RaceBoard.Common.Helpers.Pagination
{
    public class PaginatedResult<T>
    {
        #region Public Properties

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public IEnumerable<T> Results { get; set; }

        #endregion

        public PaginatedResult(int totalRecords, IPaginationFilter paginationFilter, IEnumerable<T> results)
        {
            int pageSize = totalRecords;
            int totalPages = 1;
            int pageNumber = 1;

            if (paginationFilter != null)
            {
                pageSize = paginationFilter.DisablePagination ? totalRecords : paginationFilter.PageSize;
                totalPages = paginationFilter.DisablePagination ? 1 : this.GetTotalPages(totalRecords, paginationFilter.PageSize);
                pageNumber = paginationFilter.DisablePagination ? 1 : paginationFilter.PageNumber;
            }

            this.PageSize = pageSize;
            this.PageNumber = pageNumber;
            this.TotalRecords = totalRecords;
            this.TotalPages = totalPages;
            this.Results = results;
        }

        #region Private Methods

        private int GetTotalPages(int totalRecords, int pageSize)
        {
            var totalPages = ((double)totalRecords / (double)pageSize);

            return Convert.ToInt32(Math.Ceiling(totalPages));
        }
        
        #endregion
    }
}