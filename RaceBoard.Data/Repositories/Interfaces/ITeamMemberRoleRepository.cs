using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface ITeamMemberRoleRepository
    {
        PaginatedResult<TeamMemberRole> Get(TeamMemberRoleSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
    }
}
