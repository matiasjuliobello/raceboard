//using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface IUserRoleRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);
        bool Exists(int id, ITransactionalContext? context = null);
        bool ExistsDuplicate(UserRole UserRole, ITransactionalContext? context = null);
        //PaginatedResult<UserRole> Get(UserRoleSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        //UserRole? Get(int id, ITransactionalContext? context = null);
        void Create(UserRole userRole, ITransactionalContext? context = null);
        //void Update(UserRole userRole, ITransactionalContext? context = null);
        //int Delete(UserRole userRole, ITransactionalContext? context = null);
    }
}
