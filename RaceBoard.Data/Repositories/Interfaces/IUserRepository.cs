using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);

        bool Exists(int id, ITransactionalContext? context = null);
        bool ExistsDuplicate(User user, ITransactionalContext? context = null);

        PaginatedResult<User> Get(UserSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null);
        User GetById(int id, ITransactionalContext? context = null);
        User GetByUsername(string username, ITransactionalContext? context = null);
        User GetByEmailAddress(string emailAddress, ITransactionalContext? context = null);
        void Create(User user, ITransactionalContext? context = null);
        void Update(User user, ITransactionalContext? context = null);
        int Delete(int id, ITransactionalContext? context = null);
        void SavePassword(UserPassword userPassword, ITransactionalContext? context = null);
    }
}
