using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface ICoachTeamRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);

        bool Exists(int id, ITransactionalContext? context = null);
        bool ExistsDuplicate(CoachTeam coachTeam, ITransactionalContext? context = null);

        PaginatedResult<CoachTeam> Get(CoachTeamSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        CoachTeam? Get(int id, ITransactionalContext? context = null);
        void Create(CoachTeam coachTeam, ITransactionalContext? context = null);
        void Update(CoachTeam coachTeam, ITransactionalContext? context = null);
    }
}