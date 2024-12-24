using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface ITeamMemberRoleManager
    {
        PaginatedResult<TeamMemberRole> Get(TeamMemberRoleSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        TeamMemberRole Get(int id, ITransactionalContext? context = null);
    }
}
