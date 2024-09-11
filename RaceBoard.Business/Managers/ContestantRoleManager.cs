using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Data;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Common.Enums;
using RaceBoard.Common.Exceptions;
using RaceBoard.Common.Helpers.Pagination;

namespace RaceBoard.Business.Managers
{
    public class ContestantRoleManager : AbstractManager, IContestantRoleManager
    {
        private readonly IContestantRoleRepository _contestantRoleRepository;

        #region Constructors

        public ContestantRoleManager
            (
                IContestantRoleRepository contestantRoleRepository,
                ITranslator translator
            ) : base(translator)
        {
            _contestantRoleRepository = contestantRoleRepository;
        }

        #endregion

        #region IContestantRoleManager implementation

        public PaginatedResult<ContestantRole> Get(ContestantRoleSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _contestantRoleRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public ContestantRole Get(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new ContestantRoleSearchFilter() { Ids = new int[] { id } };

            var contestantRoles = _contestantRoleRepository.Get(searchFilter: searchFilter, paginationFilter: null, sorting: null, context);

            var contestantRole = contestantRoles.Results.FirstOrDefault();
            if (contestantRole == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            return contestantRole;
        }

        #endregion
    }
}