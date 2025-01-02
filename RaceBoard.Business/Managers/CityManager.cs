using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Business.Managers
{
    public class CityManager : AbstractManager, ICityManager
    {
        private readonly ICityRepository _cityRepository;

        #region Constructors

        public CityManager
            (
                ICityRepository cityRepository,
                IRequestContextManager requestContextManager,
                ITranslator translator
            ) : base(requestContextManager, translator)
        {
            _cityRepository = cityRepository;
        }

        #endregion

        #region ICityManager implementation

        public PaginatedResult<City> Get(CitySearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _cityRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
