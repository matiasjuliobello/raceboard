using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface ITeamRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);
        bool Exists(int id, ITransactionalContext? context = null);
        bool ExistsDuplicate(Team team, ITransactionalContext? context = null);
        PaginatedResult<Team> Get(TeamSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        Team? Get(int id, ITransactionalContext? context = null);
        void Create(Team team, ITransactionalContext? context = null);
        void Update(Team team, ITransactionalContext? context = null);
        int Delete(Team team, ITransactionalContext? context = null);
    }
}
