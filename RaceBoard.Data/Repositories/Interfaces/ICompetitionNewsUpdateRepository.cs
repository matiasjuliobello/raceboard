using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface ICompetitionNewsUpdateRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);

        bool Exists(int id, ITransactionalContext? context = null);
        bool ExistsDuplicate(CompetitionNewsUpdate competitionNewsUpdate, ITransactionalContext? context = null);

        PaginatedResult<CompetitionNewsUpdate> Get(CompetitionNewsUpdateSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        void Create(CompetitionNewsUpdate competitionNewsUpdate, ITransactionalContext? context = null);
    }
}
