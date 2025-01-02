using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Business.Managers
{
    public class RoleManager : AbstractManager, IRoleManager
    {
        private readonly IRoleRepository _roleRepository;

        #region Constructors

        public RoleManager
            (
                IRoleRepository roleRepository,
                IRequestContextManager requestContextManager,
                ITranslator translator
            ) : base(requestContextManager, translator)
        {
            _roleRepository = roleRepository;
        }

        #endregion

        #region IRoleManager implementation

        public PaginatedResult<Role> Get(PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _roleRepository.Get(paginationFilter, sorting, context);
        }

        #endregion

        #region Private Methods

        #endregion
    }
}