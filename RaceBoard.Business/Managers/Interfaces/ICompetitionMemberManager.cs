using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface ICompetitionMemberManager
    {
        PaginatedResult<CompetitionMember> Get(CompetitionMemberSearchFilter searchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        CompetitionMember Get(int id, ITransactionalContext? context = null);
        PaginatedResult<CompetitionMemberInvitation> GetMemberInvitations(int idCompetition, bool isPending, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        CompetitionMemberInvitation GetInvitation(int id, ITransactionalContext? context = null);
        void AddInvitation(CompetitionMemberInvitation competitionMemberInvitation, ITransactionalContext? context = null);
        void UpdateInvitation(CompetitionMemberInvitation competitionMemberInvitation, ITransactionalContext? context = null);
        void Remove(int id, ITransactionalContext? context = null);
        void RemoveInvitation(int id, ITransactionalContext? context = null);
    }
}