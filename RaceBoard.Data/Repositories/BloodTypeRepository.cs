using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class BloodTypeRepository : AbstractRepository, IBloodTypeRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[BloodType].Id" },
            { "Name", "[BloodType].Name"}
        };

        #endregion

        #region Constructors

        public BloodTypeRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IBloodTypeRepository implementation

        public PaginatedResult<BloodType> Get(BloodTypeSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetBloodTypes(searchFilter: searchFilter, paginationFilter: paginationFilter, sorting: sorting, context: context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<BloodType> GetBloodTypes(BloodTypeSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [BloodType].Id [Id],
                                [BloodType].Name [Name]
                            FROM [BloodType] [BloodType]";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(searchFilter);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            return base.GetMultipleResultsWithPagination<BloodType>(context);
        }

        private void ProcessSearchFilter(BloodTypeSearchFilter? searchFilter = null)
        {
            base.AddFilterCriteria(ConditionType.In, "BloodType", "Id", "id", searchFilter.Ids);
            base.AddFilterCriteria(ConditionType.Like, "BloodType", "Name", "name", searchFilter.Name);
        }

        #endregion
    }
}