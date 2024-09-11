using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface ICompetitionManager
    {
        PaginatedResult<Competition> Get(CompetitionSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        Competition Get(int id, ITransactionalContext? context = null);
        void Create(Competition competition, ITransactionalContext? context = null);
        void Update(Competition competition, ITransactionalContext? context = null);
        void Delete(int id, ITransactionalContext? context = null);
    }
}
