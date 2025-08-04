using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Business.Validators.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Common.Exceptions;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;
using Enums = RaceBoard.Domain.Enums;

namespace RaceBoard.Business.Managers
{
    public class CoachTeamManager : AbstractManager, ICoachTeamManager
    {
        private readonly ICoachTeamRepository _coachTeamRepository;
        private readonly ICustomValidator<CoachTeam> _coachTeamValidator;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IAuthorizationManager _authorizationManager;

        #region Constructors

        public CoachTeamManager
            (
                ICoachTeamRepository coachTeamRepository,
                ICustomValidator<CoachTeam> coachTeamValidator,
                IRequestContextManager requestContextManager,
                IAuthorizationManager authorizationManager,
                IDateTimeHelper dateTimeHelper,
                ITranslator translator
            ) : base(requestContextManager, translator)
        {
            _coachTeamRepository = coachTeamRepository;
            _coachTeamValidator = coachTeamValidator;
            _dateTimeHelper = dateTimeHelper;
            _authorizationManager = authorizationManager;
        }

        #endregion

        #region ICoachTeamManager implementation

        public PaginatedResult<CoachTeam> Get(CoachTeamSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _coachTeamRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public CoachTeam Get(int id, ITransactionalContext? context = null)
        {
            var coachTeam = _coachTeamRepository.Get(id);
            if (coachTeam == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            return coachTeam;
        }

        public void Create(CoachTeam coachTeam, ITransactionalContext? context = null)
        {
            var contextUser = base.GetContextUser();

            coachTeam.IsActive = true;
            coachTeam.EndDate = null;

            _authorizationManager.ValidatePermission(contextUser.Id, Enums.Action.CoachTeam_Create, coachTeam.Coach.Id);

            _coachTeamValidator.SetTransactionalContext(context);

            if (!_coachTeamValidator.IsValid(coachTeam, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _coachTeamValidator.Errors);

            if (context == null)
                context = _coachTeamRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _coachTeamRepository.Create(coachTeam, context);

                _coachTeamRepository.ConfirmTransactionalContext(context);
            }
            catch (Exception)
            {
                _coachTeamRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Update(CoachTeam coachTeam, ITransactionalContext? context = null)
        {
            var contextUser = base.GetContextUser();

            _authorizationManager.ValidatePermission(contextUser.Id, Enums.Action.CoachTeam_Update, coachTeam.Coach.Id);

            if (coachTeam.EndDate == null)
                coachTeam.EndDate = _dateTimeHelper.GetCurrentTimestamp();

            _coachTeamValidator.SetTransactionalContext(context);

            if (!_coachTeamValidator.IsValid(coachTeam, Scenario.Update))
                throw new FunctionalException(ErrorType.ValidationError, _coachTeamValidator.Errors);

            if (context == null)
                context = _coachTeamRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _coachTeamRepository.Update(coachTeam, context);

                _coachTeamRepository.ConfirmTransactionalContext(context);
            }
            catch (Exception)
            {
                _coachTeamRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        #endregion
    }
}
