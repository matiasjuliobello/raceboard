using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface ITeamManager
    {
        PaginatedResult<Team> Get(TeamSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null);
        void Create(Team team, ITransactionalContext? context = null);
        void Update(Team team, ITransactionalContext? context = null);
        void Delete(int id, ITransactionalContext? context = null);

        void SetBoat(TeamBoat teamBoat, ITransactionalContext? context = null);
        void SetContestants(List<TeamContestant> teamContestants, ITransactionalContext? context = null);
    }
}