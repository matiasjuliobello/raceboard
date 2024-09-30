using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories.Interfaces
{
    public interface ICityRepository
    {
        bool Exists(int id, ITransactionalContext? context = null);

        public PaginatedResult<City> Get(CitySearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null);
    }
}