using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface ITeamMemberManager
    {
        PaginatedResult<TeamMember> Get(TeamMemberSearchFilter searchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        TeamMember Get(int id, ITransactionalContext? context = null);
        PaginatedResult<TeamMemberInvitation> GetMemberInvitations(int idTeam, bool isPending, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        TeamMemberInvitation GetInvitation(int id, ITransactionalContext? context = null);
        void AddInvitation(TeamMemberInvitation teamMemberInvitation, ITransactionalContext? context = null);
        void UpdateInvitation(TeamMemberInvitation teamMemberInvitation, ITransactionalContext? context = null);
        void Remove(int id, ITransactionalContext? context = null);
        void RemoveInvitation(int id, ITransactionalContext? context = null);
    }
}