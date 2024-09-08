using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class FlagRepository : AbstractRepository, IFlagRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[Flag].Id" },
            { "Name", "[Flag].Name"}
        };

        #endregion

        #region Constructors

        public FlagRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IFlagRepository implementation

        public bool Exists(int id, ITransactionalContext? context = null)
        {
            string existsQuery = base.GetExistsQuery("[Flag]", "[Id] = @id");

            QueryBuilder.AddCommand(existsQuery);
            QueryBuilder.AddParameter("id", id);

            return base.Execute<bool>(context);
        }

        public PaginatedResult<Flag> Get(FlagSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null)
        {
            return this.GetFlags(searchFilter: searchFilter, paginationFilter: paginationFilter, sorting: sorting, context: context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<Flag> GetFlags(FlagSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [Flag].Id [Id],
                                [Flag].Name [Name]
                            FROM [Flag] [Flag]";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            return base.GetMultipleResultsWithPagination<Flag>(context);
        }

        private void ProcessSearchFilter(FlagSearchFilter searchFilter)
        {
            base.AddFilterCriteria(ConditionType.In, "Flag", "Id", "ids", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Like, "Flag", "Name", "name", searchFilter.Name);
        }

        #endregion
    }
}