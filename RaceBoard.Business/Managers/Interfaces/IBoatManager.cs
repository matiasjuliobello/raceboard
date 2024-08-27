using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IBoatManager
    {
        PaginatedResult<Boat> Get(BoatSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null);
        void Create(Boat boat, ITransactionalContext? context = null);
        void Update(Boat boat, ITransactionalContext? context = null);
        void Delete(int id, ITransactionalContext? context = null);
    }
}
