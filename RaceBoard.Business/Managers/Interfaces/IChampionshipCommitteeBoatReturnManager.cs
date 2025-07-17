using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IChampionshipCommitteeBoatReturnManager
    {
        PaginatedResult<ChampionshipCommitteeBoatReturn> Get(ChampionshipCommitteeBoatReturnSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        ChampionshipCommitteeBoatReturn Get(int id, ITransactionalContext? context = null);
        void Create(ChampionshipCommitteeBoatReturn committeeBoatReturn, ITransactionalContext? context = null);
        void Update(ChampionshipCommitteeBoatReturn committeeBoatReturn, ITransactionalContext? context = null);
        void Delete(int id, ITransactionalContext? context = null);
    }
}
