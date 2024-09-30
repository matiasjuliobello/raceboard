using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface ICountryRepository
    {
        bool Exists(int id, ITransactionalContext? context = null);

        public PaginatedResult<Country> Get(CountrySearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
    }
}