using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IBoatOwnerManager
    {
        PaginatedResult<BoatOwner> Get(BoatOwnerSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        void Set(List<BoatOwner> boatOwners, ITransactionalContext? context = null);
    }
}
