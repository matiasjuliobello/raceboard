using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class CountryRepository : AbstractRepository, ICountryRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[Country].Id" },
            { "Name", "[Country].Name"},
            { "IsoCode", "[Country].IsoCode"}
        };

        #endregion

        #region Constructors

        public CountryRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region ICountryRepository implementation

        public bool Exists(int id, ITransactionalContext? context = null)
        {
            return base.Exists(id, "Country", "Id", context);
        }

        public PaginatedResult<Country> Get(CountrySearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetCountries(searchFilter: searchFilter, paginationFilter: paginationFilter, sorting: sorting, context: context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<Country> GetCountries(CountrySearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [Country].Id [Id],
                                [Country].Name [Name],
                                [Country].IsoCode [IsoCode]
                            FROM [Country] [Country]";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            return base.GetMultipleResultsWithPagination<Country>(context);
        }

        private void ProcessSearchFilter(CountrySearchFilter? searchFilter = null)
        {
            if (searchFilter == null)
                return;

            base.AddFilterCriteria(ConditionType.In, "Country", "Id", "ids", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Like, "Country", "Name", "name", searchFilter.Name);
        }

        #endregion
    }
}