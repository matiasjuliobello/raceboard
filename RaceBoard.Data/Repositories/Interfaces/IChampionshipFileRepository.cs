using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface IChampionshipFileRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);
        bool Exists(int id, ITransactionalContext? context = null);
        bool ExistsDuplicate(ChampionshipFile championshipFile, ITransactionalContext? context = null);
        PaginatedResult<ChampionshipFile> Get(ChampionshipFileSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        void Create(ChampionshipFile championshipFile, ITransactionalContext? context = null);
        void AssociateRaceClasses(ChampionshipFile championshipFile, ITransactionalContext? context = null);
        int Delete(int id, ITransactionalContext? context = null);
        int DeleteRaceClasses(int id, ITransactionalContext? context = null);
    }
}
