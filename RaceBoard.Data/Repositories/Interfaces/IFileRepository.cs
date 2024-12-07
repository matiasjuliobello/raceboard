using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface IFileRepository
    {
        Domain.File? Get(int id, ITransactionalContext? context = null);
        PaginatedResult<Domain.File> Get(FileSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        void Create(Domain.File file, ITransactionalContext? context = null);
        int Delete(int id, ITransactionalContext? context = null);
        int Delete(int[] ids, ITransactionalContext? context = null);
    }
}
