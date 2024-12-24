using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface ITeamMemberManager
    {
        PaginatedResult<TeamMember> Get(TeamMemberSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        TeamMember Get(int id, ITransactionalContext? context = null);
        void Create(TeamMember teamMember, ITransactionalContext? context = null);
        void Update(TeamMember teamMember, ITransactionalContext? context = null);
        void Delete(int id, ITransactionalContext? context = null);
    }
}