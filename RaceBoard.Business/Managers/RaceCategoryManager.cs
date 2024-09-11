using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Data;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data.Repositories;

namespace RaceBoard.Business.Managers
{
    public class RaceCategoryManager : AbstractManager, IRaceCategoryManager
    {
        private readonly IRaceCategoryRepository _raceCategoryRepository;

        #region Constructors

        public RaceCategoryManager
            (
                IRaceCategoryRepository raceCategoryRepository,
                ITranslator translator
            ) : base(translator)
        {
            _raceCategoryRepository = raceCategoryRepository;
        }

        #endregion

        #region IRaceCategoryManager implementation

        public PaginatedResult<RaceCategory> Get(RaceCategorySearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _raceCategoryRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        #endregion
    }
}