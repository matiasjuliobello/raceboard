using RaceBoard.Common.Enums;
using RaceBoard.Common.Exceptions;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IContestantManager
    {
        PaginatedResult<Contestant> Get(ContestantSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null);
        Contestant Get(int id, ITransactionalContext? context = null);
        void Create(Contestant contestant, ITransactionalContext? context = null);
        void Update(Contestant contestant, ITransactionalContext? context = null);
        void Delete(int id, ITransactionalContext? context = null);
    }
}
