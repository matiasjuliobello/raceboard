using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers.Interfaces
{
    public interface ICountryManager
    {
        PaginatedResult<Country> Get(CountrySearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
    }
}