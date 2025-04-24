using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class HearingRequestStatusRepository : AbstractRepository, IHearingRequestStatusRepository
    {
        #region Private Members

        private readonly Dictionary<string, string> _columnsMapping = new()
        {
            { "Id", "[RequestStatus].Id" },
            { "Name", "[RequestStatus].Name"}
        };

        #endregion

        #region Constructors

        public HearingRequestStatusRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IHearingRequestStatusRepository implementation

        public PaginatedResult<HearingRequestStatus> Get(PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return this.GetHearingRequestStatuss(paginationFilter: paginationFilter, sorting: sorting, context: context);
        }

        #endregion

        #region Private Methods

        private PaginatedResult<HearingRequestStatus> GetHearingRequestStatuss(PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [RequestStatus].Id [Id],
                                [RequestStatus].Name [Name]
                            FROM [RequestStatus] [RequestStatus]";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddSorting(sorting, _columnsMapping);
            QueryBuilder.AddPagination(paginationFilter);

            return base.GetMultipleResultsWithPagination<HearingRequestStatus>(context);
        }

        #endregion
    }
}