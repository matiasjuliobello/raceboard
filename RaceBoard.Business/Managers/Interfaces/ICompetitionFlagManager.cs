using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface ICompetitionFlagManager
    {
        PaginatedResult<CompetitionFlagGroup> GetFlags(CompetitionFlagSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        void RaiseFlags(CompetitionFlagGroup competitionFlagGroup, ITransactionalContext? context = null);
        void LowerFlags(CompetitionFlagGroup competitionFlagGroup, ITransactionalContext? context = null);
        void RemoveFlags(int id, ITransactionalContext? context = null);
    }
}
