using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IUserManager
    {
        PaginatedResult<User> Get(UserSearchFilter userSearchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        User GetById(int id, ITransactionalContext? context = null);
        User GetByUsername(string username, ITransactionalContext? context = null);
        User GetByEmailAddress(string emailAddress, ITransactionalContext? context = null);
        void Create(User user, ITransactionalContext? context = null);
        void Update(User user, ITransactionalContext? context = null);
        User Delete(int id, ITransactionalContext? context = null);
        void SavePassword(int idUser, string password, ITransactionalContext? context = null);
    }
}