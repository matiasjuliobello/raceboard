using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface ITeamContestantManager
    {
        PaginatedResult<TeamContestant> Get(TeamContestantSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        TeamContestant Get(int id, ITransactionalContext? context = null);
        void Create(TeamContestant teamContestant, ITransactionalContext? context = null);
        void Update(TeamContestant teamContestant, ITransactionalContext? context = null);
        void Delete(int id, ITransactionalContext? context = null);
    }
}