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
    public class RaceClassManager : AbstractManager, IRaceClassManager
    {
        private readonly IRaceClassRepository _raceClassRepository;

        #region Constructors

        public RaceClassManager
            (
                IRaceClassRepository raceClassRepository,
                ITranslator translator
            ) : base(translator)
        {
            _raceClassRepository = raceClassRepository;
        }

        #endregion

        #region IRaceClassManager implementation

        public PaginatedResult<RaceClass> Get(RaceClassSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null)
        {
            return _raceClassRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        #endregion
    }
}