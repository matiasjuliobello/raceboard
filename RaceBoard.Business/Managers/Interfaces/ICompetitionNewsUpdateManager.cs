using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface ICompetitionNewsUpdateManager
    {
        PaginatedResult<CompetitionNewsUpdate> Get(CompetitionNewsUpdateSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        void Create(CompetitionNewsUpdate competitionNewsUpdate, ITransactionalContext? context = null);
    }
}