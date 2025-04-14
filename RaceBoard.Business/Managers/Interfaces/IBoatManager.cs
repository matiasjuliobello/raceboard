using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IBoatManager
    {
        PaginatedResult<Boat> Search(string searchTerm, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        PaginatedResult<Boat> Get(BoatSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        Boat Get(int id, ITransactionalContext? context = null);
        void Create(Boat boat, ITransactionalContext? context = null);
        void Update(Boat boat, ITransactionalContext? context = null);
        void Delete(int id, ITransactionalContext? context = null);
    }
}
