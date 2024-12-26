using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;


namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface ICompetitionMemberRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);
        bool Exists(int id, ITransactionalContext? context = null);
        bool ExistsDuplicate(CompetitionMember competitionMember, ITransactionalContext? context = null);
        PaginatedResult<CompetitionMember> Get(CompetitionMemberSearchFilter searchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        CompetitionMember? Get(int id, ITransactionalContext? context = null);
        PaginatedResult<CompetitionMemberInvitation> GetInvitations(CompetitionMemberInvitationSearchFilter searchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        void Add(CompetitionMember competitionMember, ITransactionalContext? context = null);
        int Remove(int id, ITransactionalContext? context = null);
        void CreateInvitation(CompetitionMemberInvitation competitionMemberInvitation, ITransactionalContext? context = null);
        void UpdateInvitation(CompetitionMemberInvitation competitionMemberInvitation, ITransactionalContext? context = null);
        void RemoveInvitation(CompetitionMemberInvitation competitionMemberInvitation, ITransactionalContext? context = null);
    }
}