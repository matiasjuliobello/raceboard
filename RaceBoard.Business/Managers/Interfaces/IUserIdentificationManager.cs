using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IUserIdentificationManager
    {
        PaginatedResult<UserIdentification> Get(UserIdentificationSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        void Create(UserIdentification userIdentification, ITransactionalContext? context = null);
        void Update(UserIdentification userIdentification, ITransactionalContext? context = null);
        UserIdentification Delete(int id, ITransactionalContext? context = null);
    }
}
