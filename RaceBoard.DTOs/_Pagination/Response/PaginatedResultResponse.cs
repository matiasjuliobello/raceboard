namespace RaceBoard.DTOs._Pagination.Response
{
    public class PaginatedResultResponse<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public IEnumerable<T> Results { get; set; }
    }
}