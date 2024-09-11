using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IContestantRoleManager
    {
        PaginatedResult<ContestantRole> Get(ContestantRoleSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        ContestantRole Get(int id, ITransactionalContext? context = null);
    }
}
