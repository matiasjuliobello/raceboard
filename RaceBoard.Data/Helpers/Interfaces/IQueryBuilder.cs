using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;

namespace RaceBoard.Data.Helpers.Interfaces
{
    public interface IQueryBuilder
    {
        void AddCommand(string query);
        void AddCondition(string query);
        void AddParameter(string name, object value);
        void AddGrouping(string grouping);
        void AddSorting(string sorting);
        void AddSorting(IEnumerable<string> sorting);
        void AddSorting(Sorting sorting, Dictionary<string, string> fieldsMappings);
        void AddPagination(IPaginationFilter? paginationFilter);
        IPaginationFilter GetPaginationFilter();
        void AddReturnLastInsertedId();
        string Build();
        void Clear();
        object GetParameters();
    }
}
