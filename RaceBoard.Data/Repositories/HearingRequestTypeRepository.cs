using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class HearingRequestTypeRepository : AbstractRepository, IHearingRequestTypeRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[HearingRequestType].Id" },
            { "Name", "[HearingRequestType].Name"}
        };

        #endregion

        #region Constructors

        public HearingRequestTypeRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IHearingRequestTypeRepository implementation

        public PaginatedResult<HearingRequestType> Get(PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetHearingRequestTypes(paginationFilter: paginationFilter, sorting: sorting, context: context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<HearingRequestType> GetHearingRequestTypes(PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [HearingRequestType].Id [Id],
                                [HearingRequestType].Name [Name]
                            FROM [HearingRequestType] [HearingRequestType]";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            return base.GetMultipleResultsWithPagination<HearingRequestType>(context);
        }

        #endregion
    }
}