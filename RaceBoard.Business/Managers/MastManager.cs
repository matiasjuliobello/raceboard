using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Business.Managers
{
    public class MastManager : AbstractManager, IMastManager
    {
        private readonly IMastRepository _mastRepository;

        #region Constructors

        public MastManager
            (
                IMastRepository mastRepository,
                ITranslator translator
            ) : base(translator)
        {
            _mastRepository = mastRepository;
        }

        #endregion

        #region IMastManager implementation

        public PaginatedResult<Mast> Get(MastSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null)
        {
            return _mastRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
