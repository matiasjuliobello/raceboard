using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class TeamMemberRoleRepository : AbstractRepository, ITeamMemberRoleRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[TeamMemberRole].Id" },
            { "Name", "[TeamMemberRole].Name" }
        };

        #endregion

        #region Constructors

        public TeamMemberRoleRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region ITeamMemberRoleRepository implementation

        public PaginatedResult<TeamMemberRole> Get(TeamMemberRoleSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetTeamMemberRoles(searchFilter, paginationFilter, sorting, context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<TeamMemberRole> GetTeamMemberRoles(TeamMemberRoleSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [TeamMemberRole].Id [Id],
                                [TeamMemberRole].Name [Name]
                            FROM [TeamMemberRole] [TeamMemberRole]";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            return base.GetMultipleResultsWithPagination<TeamMemberRole>();
        }

        private void ProcessSearchFilter(TeamMemberRoleSearchFilter? searchFilter = null)
        {
            base.AddFilterCriteria(ConditionType.In, "TeamMemberRole", "Id", "ids", searchFilter?.Ids);
            base.AddFilterCriteria(ConditionType.Like, "TeamMemberRole", "Name", "name", searchFilter?.Name);
        }

        #endregion
    }
}