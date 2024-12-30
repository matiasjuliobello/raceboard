using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface IChampionshipNotificationRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);

        bool Exists(int id, ITransactionalContext? context = null);
        bool ExistsDuplicate(ChampionshipNotification championshipNotification, ITransactionalContext? context = null);
        PaginatedResult<ChampionshipNotification> Get(ChampionshipNotificationSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        void Create(ChampionshipNotification championshipNotification, ITransactionalContext? context = null);
        void AssociateRaceClasses(ChampionshipNotification championshipNotification, ITransactionalContext? context = null);
        int Delete(int id, ITransactionalContext? context = null);
        int DeleteRaceClasses(int id, ITransactionalContext? context = null);
    }
}