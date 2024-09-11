using Dapper;
using RaceBoard.Common.Extensions;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace RaceBoard.Data.Helpers
{
    public class SqlQueryBuilder : IQueryBuilder
    {
        #region Private Members

        private readonly StringBuilder _query;
        private readonly StringBuilder _totalCountQuery;
        private readonly StringBuilder _paginationQuery;

        private DynamicParameters _parameters;
        private readonly List<string> _conditions;
        private readonly List<string> _sorting;
        private readonly List<string> _grouping;
        private IPaginationFilter _paginationFilter;

        private readonly int _PAGINATION_PAGE_SIZE = 25;

        protected class SqlConstants
        {
            public const string WHERE = "WHERE";
            public const string COUNT = "COUNT";
        }

        #endregion

        #region Constructors

        public SqlQueryBuilder(IConfiguration configuration)
        {
            _query = new StringBuilder();
            _totalCountQuery = new StringBuilder();
            _paginationQuery = new StringBuilder();

            _parameters = new DynamicParameters();
            _conditions = new List<string>();
            _grouping = new List<string>();
            _sorting = new List<string>();

            _PAGINATION_PAGE_SIZE = Convert.ToInt32(configuration["Pagination_Default_PageSize"]);
        }

        #endregion

        #region IQueryBuilder implementation

        public void AddCommand(string command)
        {
            _query.AppendLine(command);
        }

        public void AddCondition(string condition)
        {
            _conditions.Add($" {GetLogicalOperator()} {condition} ");
        }

        public void AddParameter(string name, object value)
        {
            _parameters.Add(name, value);
        }

        public void AddGrouping(string grouping)
        {
            _grouping.Add(grouping);
        }

        public void AddSorting(string sorting)
        {
            _sorting.Add(sorting);
        }
        public void AddSorting(IEnumerable<string> sorting)
        {
            _sorting.AddRange(sorting);
        }
        public void AddSorting(Sorting sorting, Dictionary<string, string> fieldsMappings)
        {
            if (sorting == null)
                sorting = new Sorting() { OrderByClauses = new List<OrderByClause>() };

            if (sorting.OrderByClauses.Count() == 0)
                sorting.OrderByClauses = new List<OrderByClause>() { OrderByClause.Default };

            var orderByFields = new List<string>();
            foreach(var orderByClause in sorting.OrderByClauses)
            {
                orderByFields.Add(this.BuildOrderByClause(orderByClause));
            }

            List<string> mappedFields = this.GetMappedOrderByFields(fieldsMappings, orderByFields);

            _sorting.AddRange(mappedFields);
        }

        public void AddPagination(IPaginationFilter? paginationFilter)
        {
            if (paginationFilter == null || (paginationFilter.PageNumber == 0 && paginationFilter.PageSize == 0))
                paginationFilter = new PaginationFilter(disablePagination: true);

            if (paginationFilter.PageSize == 0)
                paginationFilter.PageSize = _PAGINATION_PAGE_SIZE;

            if (!paginationFilter.DisablePagination)
            {
                string pagination = paginationFilter.GetPaginationClause();

                _paginationQuery.Append(pagination);
            }

            _paginationFilter = paginationFilter;
        }

        public IPaginationFilter GetPaginationFilter()
        {
            return _paginationFilter;
        }

        public void AddReturnLastInsertedId()
        {
            _query.AppendLine(" (SELECT CAST(SCOPE_IDENTITY() AS INT)) ");
        }

        public string Build()
        {
            var sb = new StringBuilder();

            string sqlOriginalQuery = _query.ToString();
            sb.AppendLine(sqlOriginalQuery);

            string conditionsQuery = "";
            _conditions.ForEach(x => conditionsQuery += $" {x} ");
            sb.AppendLine(conditionsQuery);

            string grouping = this.BuildGroupingClause();
            sb.AppendLine(grouping);

            string sorting = this.BuildSortingClause();
            sb.AppendLine(sorting);

            if (_paginationQuery.Length > 0 || (_paginationFilter != null &&  _paginationFilter.DisablePagination))
            {
                string totalCountQuery = _paginationFilter.GetTotalCountQuery(sqlOriginalQuery, conditionsQuery.RemoveFirstInstanceOfString(SqlConstants.WHERE));

                sb.Insert(0, totalCountQuery);
                sb.Append(_paginationQuery.ToString());
            }

            string query = sb.ToString();

            Console.WriteLine(query);

            return query;
        }

        public void Clear()
        {
            ClearMainQuery();

            ClearPagination();

            ClearGrouping();

            ClearSorting();
        }

        public object GetParameters()
        {
            return _parameters;
        }

        #endregion

        #region Private Methods

        private void ClearMainQuery()
        {
            _query.Clear();
            _conditions.Clear();
            _parameters = new DynamicParameters();
        
        }
        private void ClearPagination()
        {
            _totalCountQuery.Clear();
            _paginationQuery.Clear();
            _paginationFilter = null;
        }

        private void ClearGrouping()
        {
            _grouping.Clear();
        }

        private void ClearSorting()
        {
            _sorting.Clear();
        }

        private string GetLogicalOperator()
        {
            return _conditions.Any() ? " AND " : " WHERE ";
        }

        private string BuildOrderByClause(OrderByClause orderByClause)
        {
            return $"{orderByClause.ColumnName} {(orderByClause.Direction == OrderByDirection.Descending ? "DESC" : "ASC")}";
        }

        private string BuildGroupingClause()
        {
            if (_grouping.Any())
                return "GROUP BY " + String.Join(", ", _grouping);

            return string.Empty;
        }

        private string BuildSortingClause()
        {
            if (_sorting.Any())
                return "ORDER BY " + String.Join(", ", _sorting);

            return string.Empty;
        }

        private List<string> GetMappedOrderByFields(Dictionary<string, string> fieldsDictionary, List<string> orderByFields)
        {
            fieldsDictionary = new Dictionary<string, string>(fieldsDictionary, StringComparer.InvariantCultureIgnoreCase);

            return orderByFields.Select(orderField =>
            {
                (string order, string direction) = this.GetOrderField(orderField);

                if (fieldsDictionary.TryGetValue(order, out string mappedField))
                {
                    return $"{mappedField} {direction}";
                }
                return order;

            }).Where(x => x != null).ToList();
        }

        private (string order, string direction) GetOrderField(string orderString)
        {
            var orderField = orderString.Split(' ');
            var field = "";
            var direction = "";
            if (orderField.Length > 0)
            {
                field = orderField[0];
            }
            if (orderField.Length > 1)
            {
                direction = orderField[1];
            }
            return (field, direction);
        }

        #endregion
    }
}
