using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IOrganizationManager
    {
        PaginatedResult<Organization> Get(OrganizationSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        Organization Get(int id, ITransactionalContext? context = null);
        void Create(Organization organization, ITransactionalContext? context = null);
        void Update(Organization organization, ITransactionalContext? context = null);
        void Delete(int id, ITransactionalContext? context = null);
    }
}
