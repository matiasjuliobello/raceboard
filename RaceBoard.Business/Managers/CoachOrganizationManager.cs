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
using System.Diagnostics;
using Enums = RaceBoard.Domain.Enums;

namespace RaceBoard.Business.Managers
{
    public class CoachOrganizationManager : AbstractManager, ICoachOrganizationManager
    {
        private readonly ICoachOrganizationRepository _coachOrganizationRepository;
        private readonly ICustomValidator<CoachOrganization> _coachOrganizationValidator;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IAuthorizationManager _authorizationManager;

        #region Constructors

        public CoachOrganizationManager
            (
                ICoachOrganizationRepository coachOrganizationRepository,
                ICustomValidator<CoachOrganization> coachOrganizationValidator,
                IRequestContextManager requestContextManager,
                IAuthorizationManager authorizationManager,
                IDateTimeHelper dateTimeHelper,
                ITranslator translator
            ) : base(requestContextManager, translator)
        {
            _coachOrganizationRepository = coachOrganizationRepository;
            _coachOrganizationValidator = coachOrganizationValidator;
            _dateTimeHelper = dateTimeHelper;

            _authorizationManager = authorizationManager;
        }

        #endregion

        #region ICoachOrganizationManager implementation

        public PaginatedResult<CoachOrganization> Get(CoachOrganizationSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _coachOrganizationRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public CoachOrganization Get(int id, ITransactionalContext? context = null)
        {
            var coachOrganization = _coachOrganizationRepository.Get(id);
            if (coachOrganization == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            return coachOrganization;
        }

        public void Create(CoachOrganization coachOrganization, ITransactionalContext? context = null)
        {
            var contextUser = base.GetContextUser();

            coachOrganization.IsActive = true;
            coachOrganization.EndDate = null;
            
            _authorizationManager.ValidatePermission(contextUser.Id, Enums.Action.CoachOrganization_Create, coachOrganization.Coach.Id);

            _coachOrganizationValidator.SetTransactionalContext(context);

            if (!_coachOrganizationValidator.IsValid(coachOrganization, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _coachOrganizationValidator.Errors);

            if (context == null)
                context = _coachOrganizationRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _coachOrganizationRepository.Create(coachOrganization, context);

                _coachOrganizationRepository.ConfirmTransactionalContext(context);
            }
            catch (Exception)
            {
                _coachOrganizationRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Update(CoachOrganization coachOrganization, ITransactionalContext? context = null)
        {
            var contextUser = base.GetContextUser();

            _authorizationManager.ValidatePermission(contextUser.Id, Enums.Action.CoachOrganization_Update, coachOrganization.Coach.Id);

            if (coachOrganization.EndDate == null)
                coachOrganization.EndDate = _dateTimeHelper.GetCurrentTimestamp();

            _coachOrganizationValidator.SetTransactionalContext(context);

            if (!_coachOrganizationValidator.IsValid(coachOrganization, Scenario.Update))
                throw new FunctionalException(ErrorType.ValidationError, _coachOrganizationValidator.Errors);

            if (context == null)
                context = _coachOrganizationRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _coachOrganizationRepository.Update(coachOrganization, context);

                _coachOrganizationRepository.ConfirmTransactionalContext(context);
            }
            catch (Exception)
            {
                _coachOrganizationRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        #endregion
    }
}
