using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IChampionshipFlagManager
    {
        PaginatedResult<ChampionshipFlagGroup> GetFlags(ChampionshipFlagSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        ChampionshipFlagGroup Get(int id, ITransactionalContext? context = null);
        void RaiseFlags(ChampionshipFlagGroup championshipFlagGroup, ITransactionalContext? context = null);
        void LowerFlags(ChampionshipFlagGroup championshipFlagGroup, ITransactionalContext? context = null);
        void RemoveFlags(int id, ITransactionalContext? context = null);
    }
}
