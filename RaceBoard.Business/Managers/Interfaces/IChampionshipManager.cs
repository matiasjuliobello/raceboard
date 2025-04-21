using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IChampionshipManager
    {
        PaginatedResult<Championship> Get(ChampionshipSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        Championship Get(int id, ITransactionalContext? context = null);
        void Create(Championship championship, ITransactionalContext? context = null);
        void Update(Championship championship, ITransactionalContext? context = null);
        void Delete(int id, ITransactionalContext? context = null);

        List<ChampionshipGroup> GetGroups(int idChampionship, ITransactionalContext? context = null);
        ChampionshipGroup GetGroup(int id, ITransactionalContext? context = null);
        void CreateGroup(ChampionshipGroup championshipGroup, ITransactionalContext? context = null);
        void UpdateGroup(ChampionshipGroup championshipGroup, ITransactionalContext? context = null);
        void DeleteGroup(int idChampionshipGroup, ITransactionalContext? context = null);

        PaginatedResult<CommitteeBoatReturn> GetCommitteeBoatReturns(CommitteeBoatReturnSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        void CreateCommitteeBoatReturn(CommitteeBoatReturn committeeBoatReturn, ITransactionalContext? context = null);
        void DeleteCommitteeBoatReturn(int id, ITransactionalContext? context = null);

        void CreateProtest(RaceProtest raceProtest, ITransactionalContext? context = null);
    }
}
