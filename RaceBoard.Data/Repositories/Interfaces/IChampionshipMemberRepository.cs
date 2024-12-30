using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;


namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface IChampionshipMemberRepository
    {
        ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal);
        void ConfirmTransactionalContext(ITransactionalContext context);
        void CancelTransactionalContext(ITransactionalContext context);
        bool Exists(int id, ITransactionalContext? context = null);
        bool ExistsDuplicate(ChampionshipMember championshipMember, ITransactionalContext? context = null);
        PaginatedResult<ChampionshipMember> Get(ChampionshipMemberSearchFilter searchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        ChampionshipMember? Get(int id, ITransactionalContext? context = null);
        PaginatedResult<ChampionshipMemberInvitation> GetInvitations(ChampionshipMemberInvitationSearchFilter searchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        void Add(ChampionshipMember championshipMember, ITransactionalContext? context = null);
        int Remove(int id, ITransactionalContext? context = null);
        void CreateInvitation(ChampionshipMemberInvitation championshipMemberInvitation, ITransactionalContext? context = null);
        void UpdateInvitation(ChampionshipMemberInvitation championshipMemberInvitation, ITransactionalContext? context = null);
        void RemoveInvitation(ChampionshipMemberInvitation championshipMemberInvitation, ITransactionalContext? context = null);
    }
}