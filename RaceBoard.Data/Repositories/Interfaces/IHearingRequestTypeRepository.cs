using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;


namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface IHearingRequestTypeRepository
    {
        public PaginatedResult<HearingRequestType> Get(PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
    }
}
