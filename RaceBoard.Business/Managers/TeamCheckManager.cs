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
        private readonly ITeamMemberCheckRepository _teamCheckRepository;
        private readonly ICustomValidator<TeamMemberCheck> _teamCheckValidator;
        private readonly IDateTimeHelper _dateTimeHelper;

        #region Constructors

        public TeamCheckManager
            (
                ITeamMemberCheckRepository teamCheckRepository,
                ICustomValidator<TeamMemberCheck> teamCheckValidator,
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

        public PaginatedResult<TeamMemberCheck> Get(TeamCheckSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _teamCheckRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public void Create(TeamMemberCheck teamMemberCheck, ITransactionalContext? context = null)
        {
            _teamCheckValidator.SetTransactionalContext(context);

            teamMemberCheck.CheckTime = _dateTimeHelper.GetCurrentTimestamp();

            if (!_teamCheckValidator.IsValid(teamMemberCheck, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _teamCheckValidator.Errors);

            if (context == null)
                context = _teamCheckRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _teamCheckRepository.Create(teamMemberCheck, context);

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
