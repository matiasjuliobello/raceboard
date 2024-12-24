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
        PaginatedResult<TeamMember> Get(TeamMemberSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        TeamMember? Get(int id, ITransactionalContext? context = null);
        void Create(TeamMember teamMember, ITransactionalContext? context = null);
        void Update(TeamMember teamMember, ITransactionalContext? context = null);
        void Delete(TeamMember teamMember, ITransactionalContext? context = null);
    }
}