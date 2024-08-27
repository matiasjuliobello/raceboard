using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface IBoatRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);

        bool Exists(int id, ITransactionalContext? context = null);
        bool ExistsDuplicate(Boat boat, ITransactionalContext? context = null);

        PaginatedResult<Boat> Get(BoatSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null);
        void Create(Boat boat, ITransactionalContext? context = null);
        void Update(Boat boat, ITransactionalContext? context = null);
        int Delete(int id, ITransactionalContext? context = null);
    }
}
