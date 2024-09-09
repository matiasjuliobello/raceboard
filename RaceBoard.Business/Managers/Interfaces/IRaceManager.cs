using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IRaceManager
    {
        PaginatedResult<Race> Get(RaceSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null);
        void Create(Race race, ITransactionalContext? context = null);
        void Update(Race race, ITransactionalContext? context = null);
        void Delete(int id, ITransactionalContext? context = null);
    }
}
