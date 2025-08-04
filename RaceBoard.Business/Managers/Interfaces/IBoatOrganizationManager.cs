using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IBoatOrganizationManager
    {
        PaginatedResult<BoatOrganization> Get(BoatOrganizationSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        void Set(List<BoatOrganization> boatOrganizations, ITransactionalContext? context = null);
    }
}
