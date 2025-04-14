using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using static RaceBoard.Data.Helpers.SqlQueryBuilder;

namespace RaceBoard.Data.Helpers.Interfaces
{
    public interface IQueryBuilder
    {
        void AddCommand(string query);
        void AddConditionGroup(string? logicalOperator = LogicalOperator.And);
        void AddCondition(string condition, string? logicalOperator = LogicalOperator.And);
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
