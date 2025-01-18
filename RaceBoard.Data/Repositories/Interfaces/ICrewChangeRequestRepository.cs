using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface ICrewChangeRequestRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);
        bool Exists(int id, ITransactionalContext? context = null);
        bool ExistsDuplicate(CrewChangeRequest crewChangeRequest, ITransactionalContext? context = null);
        PaginatedResult<CrewChangeRequest> Get(ChangeRequestSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        CrewChangeRequest? Get(int id, ITransactionalContext? context = null);
        void Create(CrewChangeRequest crewChangeRequest, ITransactionalContext? context = null);
    }
}