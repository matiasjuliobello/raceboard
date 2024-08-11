namespace RaceBoard.Common.Helpers.Interfaces
{
    public interface IPaginationFilter
    {
        int PageNumber { get; set; }
        int PageSize { get; set; }
        bool DisablePagination { get; set; }

        string GetPaginationClause();
        string GetTotalCountQuery(string query, string conditions);
    }
}
