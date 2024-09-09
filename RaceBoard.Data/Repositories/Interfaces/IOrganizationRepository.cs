using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface IOrganizationRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);

        PaginatedResult<Organization> Get(OrganizationSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null);
        void Create(Organization organization, ITransactionalContext? context = null);
        void Update(Organization organization, ITransactionalContext? context = null);
        int Delete(int id, ITransactionalContext? context = null);
    }
}
