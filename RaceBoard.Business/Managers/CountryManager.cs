using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Business.Managers
{
    public class CountryManager : AbstractManager, ICountryManager
    {
        private readonly ICountryRepository _countryRepository;

        #region Constructors

        public CountryManager
            (
                ICountryRepository countryRepository,
                IRequestContextManager requestContextManager,
                ITranslator translator
            ) : base(requestContextManager, translator)
        {
            _countryRepository = countryRepository;
        }

        #endregion

        #region ICountryManager implementation

        public PaginatedResult<Country> Get(CountrySearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _countryRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
