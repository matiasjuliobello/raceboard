using RaceBoard.Common.Helpers.Interfaces;

namespace RaceBoard.Common.Helpers.Pagination
{
    public class PaginationFilter : IPaginationFilter
    {
        #region Private Members

        private int _pageSize;
        private int _pageNumber;
        private bool _disablePagination = false;

        #endregion

        #region Constructors

        public PaginationFilter(bool disablePagination)
        {
            _disablePagination = disablePagination;
        }
        public PaginationFilter(int pageNumber, int pageSize, bool disablePagination = false)
        {
            _pageNumber = pageNumber;
            _pageSize = pageSize;
            _disablePagination = disablePagination;
        }

        #endregion

        #region IPaginationFilter implementation

        public int PageNumber
        {
            get { return _pageNumber; }
            set { _pageNumber = value; }
        }

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        public bool DisablePagination
        {
            get { return _disablePagination; }
            set { _disablePagination = value; }
        }

        public string GetPaginationClause()
        {
            return $" OFFSET {this.CalculateOffset()} ROWS FETCH NEXT {this.CalculateFetch()} ROWS ONLY;";
        }

        public string GetTotalCountQuery(string query, string conditions)
        {
            string _query = query;

            int indexOfFrom = 0;
            int indexOfApply = 0;
            int indexOfJoin = 0;

            while (true)
            {
                indexOfFrom = _query.LastIndexOf("FROM", StringComparison.InvariantCultureIgnoreCase);
                indexOfApply = _query.LastIndexOf("APPLY", StringComparison.InvariantCultureIgnoreCase);
                indexOfJoin = _query.LastIndexOf("JOIN", StringComparison.InvariantCultureIgnoreCase);

                if (indexOfApply == -1 && indexOfJoin == -1)
                    break;

                bool condition1 = ((indexOfFrom > indexOfApply) && (indexOfApply >= 0));
                bool condition2 = (indexOfFrom > indexOfJoin);

                if (condition1 || condition2)
                    _query = _query.Substring(0, indexOfFrom);
                else
                    break;
            }

            var fromTable = query[indexOfFrom..];

            return @$"  SELECT COUNT(*) {fromTable}
                        WHERE EXISTS
                        (
                            SELECT 1 {fromTable} {(string.IsNullOrEmpty(conditions) ? "" : $" WHERE {conditions}")}
                        ) {(string.IsNullOrEmpty(conditions) ? "" : $" AND {conditions}")};";
        }

        #endregion

        #region Private Methods

        private int CalculateOffset()
        {
            return _pageNumber < 1 ? 0 : (_pageNumber * _pageSize);
        }

        private int CalculateFetch()
        {
            return _pageSize;
        }

        #endregion
    }
}
