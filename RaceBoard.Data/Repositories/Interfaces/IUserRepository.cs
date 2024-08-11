using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        PaginatedResult<User> Get(UserSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null);
        User GetById(int id, ITransactionalContext? context = null);
        User GetByUsername(string username, ITransactionalContext? context = null);
        User GetByEmailAddress(string emailAddress, ITransactionalContext? context = null);
        void Create(User user, ITransactionalContext? context = null);
        void Update(User user, ITransactionalContext? context = null);
        int Delete(int id, ITransactionalContext? context = null);
        void SavePassword(UserPassword userPassword, ITransactionalContext? context = null);

        bool Exists(int id, ITransactionalContext? context = null);
        bool ExistsDuplicate(User user, ITransactionalContext? context = null);
    }
}
