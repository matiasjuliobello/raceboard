using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Business.Validators.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Common.Exceptions;
using RaceBoard.Common.Helpers;
using RaceBoard.Common.Helpers.Interfaces;
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
        private readonly IDateTimeHelper _dateTimeHelper;

        #region Constructors

        public TeamCheckManager
            (
                ITeamCheckRepository teamCheckRepository,
                ICustomValidator<TeamContestantCheck> teamCheckValidator,
                IDateTimeHelper dateTimeHelper,
                ITranslator translator
            ) : base(translator)
        {
            _teamCheckRepository = teamCheckRepository;
            _teamCheckValidator = teamCheckValidator;
            _dateTimeHelper = dateTimeHelper;
        }

        #endregion

        #region ITeamCheckManager implementation

        public PaginatedResult<TeamContestantCheck> Get(TeamCheckSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _teamCheckRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public void Create(TeamContestantCheck teamContestantCheck, ITransactionalContext? context = null)
        {
            _teamCheckValidator.SetTransactionalContext(context);

            teamContestantCheck.CheckTime = _dateTimeHelper.GetCurrentTimestamp();

            if (!_teamCheckValidator.IsValid(teamContestantCheck, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _teamCheckValidator.Errors);

            if (context == null)
                context = _teamCheckRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _teamCheckRepository.Create(teamContestantCheck, context);

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
