using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface ITeamManager
    {
        PaginatedResult<Team> Get(TeamSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        Team Get(int id, ITransactionalContext? context = null);
        void Create(Team team, ITransactionalContext? context = null);
        void Update(Team team, ITransactionalContext? context = null);
        void Delete(int id, ITransactionalContext? context = null);
    }
}