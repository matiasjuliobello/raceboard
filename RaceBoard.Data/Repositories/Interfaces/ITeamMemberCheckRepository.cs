using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface ITeamMemberCheckRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);
        bool Exists(int id, ITransactionalContext? context = null);
        bool ExistsDuplicate(TeamMemberCheck teamCheck, ITransactionalContext? context = null);

        PaginatedResult<TeamMemberCheck> Get(TeamCheckSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        void Create(TeamMemberCheck teamCheck, ITransactionalContext? context = null);
    }
}