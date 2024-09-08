using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class ContestantRoleRepository : AbstractRepository, IContestantRoleRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[ContestantRole].Id" },
            { "Name", "[ContestantRole].Name" }
        };

        #endregion

        #region Constructors

        public ContestantRoleRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IContestantRoleRepository implementation

        public PaginatedResult<ContestantRole> Get(ContestantRoleSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null)
        {
            return this.GetContestantRoles(searchFilter, paginationFilter, sorting, context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<ContestantRole> GetContestantRoles(ContestantRoleSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [ContestantRole].Id [Id],
                                [ContestantRole].Name [Name]
                            FROM [ContestantRole] [ContestantRole]";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            return base.GetMultipleResultsWithPagination<ContestantRole>();
        }

        private void ProcessSearchFilter(ContestantRoleSearchFilter searchFilter)
        {
            base.AddFilterCriteria(ConditionType.In, "ContestantRole", "Id", "ids", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Like, "ContestantRole", "Name", "name", searchFilter.Name);
        }

        #endregion
    }
}