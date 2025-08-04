using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface ICoachOrganizationRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);

        bool Exists(int id, ITransactionalContext? context = null);
        bool ExistsDuplicate(CoachOrganization coachOrganization, ITransactionalContext? context = null);

        PaginatedResult<CoachOrganization> Get(CoachOrganizationSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        CoachOrganization? Get(int id, ITransactionalContext? context = null);
        void Create(CoachOrganization coachOrganization, ITransactionalContext? context = null);
        void Update(CoachOrganization coachOrganization, ITransactionalContext? context = null);
    }
}