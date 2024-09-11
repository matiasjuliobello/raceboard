using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface ITeamContestantRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);
        bool Exists(int id, ITransactionalContext? context = null);
        bool ExistsDuplicate(TeamContestant teamContestant, ITransactionalContext? context = null);
        PaginatedResult<TeamContestant> Get(TeamContestantSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        TeamContestant? Get(int id, ITransactionalContext? context = null);
        void Create(TeamContestant teamContestant, ITransactionalContext? context = null);
        void Update(TeamContestant teamContestant, ITransactionalContext? context = null);
        void Delete(TeamContestant teamContestant, ITransactionalContext? context = null);
    }
}