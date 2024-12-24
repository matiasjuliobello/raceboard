using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class RoleRepository : AbstractRepository, IRoleRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[Role].Id" },
            { "Name", "[Role].Name"}
        };

        #endregion

        #region Constructors

        public RoleRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IRoleRepository implementation

        public PaginatedResult<Role> Get(PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetRoles(paginationFilter: paginationFilter, sorting: sorting, context: context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<Role> GetRoles(PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [Role].Id [Id],
                                [Role].Name [Name]
                            FROM [Role] [Role]";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            return base.GetMultipleResultsWithPagination<Role>(context);
        }

        #endregion
    }
}