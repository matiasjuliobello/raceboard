using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface ITeamCheckManager
    {
        PaginatedResult<TeamContestantCheck> Get(TeamCheckSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        void Create(TeamContestantCheck teamCheck, ITransactionalContext? context = null);
    }
}
