using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface ITeamCheckManager
    {
        PaginatedResult<TeamMemberCheck> Get(TeamCheckSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        void Create(TeamMemberCheck teamCheck, ITransactionalContext? context = null);
    }
}
