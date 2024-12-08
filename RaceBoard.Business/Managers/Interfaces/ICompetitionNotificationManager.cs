using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface ICompetitionNotificationManager
    {
        PaginatedResult<CompetitionNotification> Get(CompetitionNotificationSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        CompetitionNotification Get(int id, ITransactionalContext? context = null);
        void Create(CompetitionNotification competitionNotification, ITransactionalContext? context = null);
        void Delete(int id, ITransactionalContext? context = null);
    }
}