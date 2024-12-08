using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface ICompetitionNotificationRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);

        bool Exists(int id, ITransactionalContext? context = null);
        bool ExistsDuplicate(CompetitionNotification competitionNotification, ITransactionalContext? context = null);
        PaginatedResult<CompetitionNotification> Get(CompetitionNotificationSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        void Create(CompetitionNotification competitionNotification, ITransactionalContext? context = null);
        void AssociateRaceClasses(CompetitionNotification competitionNotification, ITransactionalContext? context = null);
        int Delete(int id, ITransactionalContext? context = null);
        int DeleteRaceClasses(int id, ITransactionalContext? context = null);
    }
}