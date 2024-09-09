using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IOrganizationManager
    {
        PaginatedResult<Organization> Get(OrganizationSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null);
        void Create(Organization organization, ITransactionalContext? context = null);
        void Update(Organization organization, ITransactionalContext? context = null);
        void Delete(int id, ITransactionalContext? context = null);
    }
}
