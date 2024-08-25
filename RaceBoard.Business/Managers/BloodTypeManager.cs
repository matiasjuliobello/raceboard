using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Business.Managers
{
    public class BloodTypeManager : AbstractManager, IBloodTypeManager
    {
        private readonly IBloodTypeRepository _bloodTypeRepository;

        #region Constructors

        public BloodTypeManager
            (
                IBloodTypeRepository bloodTypeRepository,
                ITranslator translator
            ) : base(translator)
        {
            _bloodTypeRepository = bloodTypeRepository;
        }

        #endregion

        #region IBloodTypeManager implementation

        public PaginatedResult<BloodType> Get(BloodTypeSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null)
        {
            return _bloodTypeRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
