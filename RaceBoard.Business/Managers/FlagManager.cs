using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Business.Managers
{
    public class FlagManager : AbstractManager, IFlagManager
    {
        private readonly IFlagRepository _flagRepository;

        #region Constructors

        public FlagManager
            (
                IFlagRepository flagRepository,
                ITranslator translator
            ) : base(translator)
        {
            _flagRepository = flagRepository;
        }

        #endregion

        #region IFlagManager implementation

        public PaginatedResult<Flag> Get(FlagSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null)
        {
            return _flagRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
