using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface IContestantRoleManager
    {
        PaginatedResult<ContestantRole> Get(ContestantRoleSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null);
        ContestantRole Get(int id, ITransactionalContext? context = null);
    }
}
