using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface IContestantRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);
        bool Exists(int id, ITransactionalContext? context = null);
        bool ExistsDuplicate(Contestant contestant, ITransactionalContext? context = null);
        PaginatedResult<Contestant> Get(ContestantSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null);
        void Create(Contestant contestant, ITransactionalContext? context = null);
        void Update(Contestant contestant, ITransactionalContext? context = null);
        int Delete(int id, ITransactionalContext? context = null);
    }
}
