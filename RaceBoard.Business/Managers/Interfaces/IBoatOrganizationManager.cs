using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IBoatOrganizationManager
    {
        PaginatedResult<BoatOrganization> Get(BoatOrganizationSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        BoatOrganization Get(int id, ITransactionalContext? context = null);
        void Create(BoatOrganization boatOrganization, ITransactionalContext? context = null);
        void Update(BoatOrganization boatOrganization, ITransactionalContext? context = null);
        void Delete(int id, ITransactionalContext? context = null);
    }
}
