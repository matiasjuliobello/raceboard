using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface ICoachRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);

        bool Exists(int id, ITransactionalContext? context = null);
        bool ExistsDuplicate(Coach coach, ITransactionalContext? context = null);

        PaginatedResult<Coach> Get(CoachSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        Coach? Get(int id, ITransactionalContext? context = null);
        void Create(Coach coach, ITransactionalContext? context = null);
        void Delete(int id, ITransactionalContext? context = null);
    }
}