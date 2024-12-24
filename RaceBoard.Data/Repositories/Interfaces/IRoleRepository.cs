using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        public PaginatedResult<Role> Get(PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
    }
}
