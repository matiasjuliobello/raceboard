using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface ITeamBoatManager
    {
        PaginatedResult<TeamBoat> Get(TeamBoatSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        TeamBoat Get(int id, ITransactionalContext? context = null);
        void Create(TeamBoat teamBoat, ITransactionalContext? context = null);
        void Update(TeamBoat teamBoat, ITransactionalContext? context = null);
        void Delete(int id, ITransactionalContext? context = null);
    }
}
