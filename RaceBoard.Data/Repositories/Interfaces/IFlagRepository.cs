using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface IFlagRepository
    {
        bool Exists(int id, ITransactionalContext? context = null);

        public PaginatedResult<Flag> Get(FlagSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null);
    }
}
