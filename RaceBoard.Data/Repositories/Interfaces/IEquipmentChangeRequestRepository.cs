using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface IEquipmentChangeRequestRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);
        bool Exists(int id, ITransactionalContext? context = null);
        bool ExistsDuplicate(EquipmentChangeRequest equipmentChangeRequest, ITransactionalContext? context = null);
        PaginatedResult<EquipmentChangeRequest> Get(ChangeRequestSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        EquipmentChangeRequest? Get(int id, ITransactionalContext? context = null);
        void Create(EquipmentChangeRequest equipmentChangeRequest, ITransactionalContext? context = null);
    }
}