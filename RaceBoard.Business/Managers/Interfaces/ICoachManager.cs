using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface ICoachManager
    {
        PaginatedResult<Coach> Get(CoachSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        Coach Get(int id, ITransactionalContext? context = null);
        void Create(Coach coach, ITransactionalContext? context = null);
        void Delete(int id, ITransactionalContext? context = null);
    }
}