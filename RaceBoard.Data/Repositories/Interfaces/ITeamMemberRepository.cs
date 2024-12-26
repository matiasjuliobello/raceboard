using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface ITeamMemberRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);
        bool Exists(int id, ITransactionalContext? context = null);
        bool ExistsDuplicate(TeamMember teamMember, ITransactionalContext? context = null);
        bool HasMemberInAnotherCompetitionTeam(TeamMember teamMember, ITransactionalContext? context = null);
        bool HasParticipationOnRace(TeamMember teamMember, ITransactionalContext? context = null);
        bool HasDuplicatedRole(TeamMember teamMember, ITransactionalContext? context = null);
        PaginatedResult<TeamMember> Get(TeamMemberSearchFilter searchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        PaginatedResult<TeamMemberInvitation> GetInvitations(TeamMemberInvitationSearchFilter searchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        void Add(TeamMember teamMember, ITransactionalContext? context = null);
        int Remove(int id, ITransactionalContext? context = null);
        void CreateInvitation(TeamMemberInvitation teamMemberInvitation, ITransactionalContext? context = null);
        void UpdateInvitation(TeamMemberInvitation teamMemberInvitation, ITransactionalContext? context = null);
        void RemoveInvitation(TeamMemberInvitation teamMemberInvitation, ITransactionalContext? context = null);
    }
}