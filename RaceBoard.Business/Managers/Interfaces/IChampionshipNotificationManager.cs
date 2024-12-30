using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IChampionshipNotificationManager
    {
        PaginatedResult<ChampionshipNotification> Get(ChampionshipNotificationSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        ChampionshipNotification Get(int id, ITransactionalContext? context = null);
        void Create(ChampionshipNotification championshipNotification, ITransactionalContext? context = null);
        void Delete(int id, ITransactionalContext? context = null);
    }
}