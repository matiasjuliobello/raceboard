using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IChampionshipMemberManager
    {
        PaginatedResult<ChampionshipMember> Get(ChampionshipMemberSearchFilter searchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        ChampionshipMember Get(int id, ITransactionalContext? context = null);
        PaginatedResult<ChampionshipMemberInvitation> GetMemberInvitations(int idChampionship, bool isPending, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        ChampionshipMemberInvitation GetInvitation(int id, ITransactionalContext? context = null);
        void CreateInvitation(ChampionshipMemberInvitation championshipMemberInvitation, ITransactionalContext? context = null);
        void UpdateInvitation(ChampionshipMemberInvitation championshipMemberInvitation, ITransactionalContext? context = null);
        void Remove(int id, ITransactionalContext? context = null);
        void RemoveInvitation(int id, ITransactionalContext? context = null);
    }
}