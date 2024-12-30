using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface IChampionshipFlagRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);
        bool Exists(int id, ITransactionalContext? context = null);
        bool ExistsDuplicate(ChampionshipFlagGroup championshipFlagGroup, ITransactionalContext? context = null);

        PaginatedResult<ChampionshipFlagGroup> Get(ChampionshipFlagSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        void CreateGroup(ChampionshipFlagGroup championshipFlagGroup, ITransactionalContext? context = null);
        void AddFlags(List<ChampionshipFlag> championshipFlags, ITransactionalContext? context = null);
        void UpdateFlags(List<ChampionshipFlag> championshipFlags, ITransactionalContext? context = null);
        int DeleteGroup(int id, ITransactionalContext? context = null);
    }
}
