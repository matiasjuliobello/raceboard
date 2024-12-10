using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface ICompetitionFlagRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);
        bool Exists(int id, ITransactionalContext? context = null);
        bool ExistsDuplicate(CompetitionFlagGroup competitionFlagGroup, ITransactionalContext? context = null);

        PaginatedResult<CompetitionFlagGroup> Get(CompetitionFlagSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        void CreateGroup(CompetitionFlagGroup competitionFlagGroup, ITransactionalContext? context = null);
        void AddFlags(List<CompetitionFlag> competitionFlags, ITransactionalContext? context = null);
        void UpdateFlags(List<CompetitionFlag> competitionFlags, ITransactionalContext? context = null);
        int DeleteGroup(int id, ITransactionalContext? context = null);
    }
}
