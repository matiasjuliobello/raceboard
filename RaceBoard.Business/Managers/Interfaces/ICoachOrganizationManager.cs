using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface ICoachOrganizationManager
    {
        PaginatedResult<CoachOrganization> Get(CoachOrganizationSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
        CoachOrganization Get(int id, ITransactionalContext? context = null);
        void Create(CoachOrganization coachOrganization, ITransactionalContext? context = null);
        void Update(CoachOrganization coachOrganization, ITransactionalContext? context = null);
    }
}
