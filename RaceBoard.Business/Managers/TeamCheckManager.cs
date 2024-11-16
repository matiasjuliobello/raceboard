using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Business.Validators.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Common.Exceptions;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Business.Managers
{
    public class TeamCheckManager : AbstractManager, ITeamCheckManager
    {
        private readonly ITeamCheckRepository _teamCheckRepository;
        private readonly ICustomValidator<TeamContestantCheck> _teamCheckValidator;

        #region Constructors

        public TeamCheckManager
            (
                ITeamCheckRepository teamCheckRepository,
                ICustomValidator<TeamContestantCheck> teamCheckValidator,
                ITranslator translator
            ) : base(translator)
        {
            _teamCheckRepository = teamCheckRepository;
            _teamCheckValidator = teamCheckValidator;
        }

        #endregion

        #region ITeamCheckManager implementation

        public PaginatedResult<TeamContestantCheck> Get(TeamCheckSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _teamCheckRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public void Create(TeamContestantCheck teamCheck, ITransactionalContext? context = null)
        {
            _teamCheckValidator.SetTransactionalContext(context);

            if (!_teamCheckValidator.IsValid(teamCheck, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _teamCheckValidator.Errors);

            if (context == null)
                context = _teamCheckRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _teamCheckRepository.Create(teamCheck, context);

                _teamCheckRepository.ConfirmTransactionalContext(context);
            }
            catch (Exception)
            {
                _teamCheckRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        #endregion
    }
}
