using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface ICoachTeamManager
    {
        PaginatedResult<CoachTeam> Get(CoachTeamSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        CoachTeam Get(int id, ITransactionalContext? context = null);
        void Create(CoachTeam coachTeam, ITransactionalContext? context = null);
        void Update(CoachTeam coachTeam, ITransactionalContext? context = null);
    }
}
