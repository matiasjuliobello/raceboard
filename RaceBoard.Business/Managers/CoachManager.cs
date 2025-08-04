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

namespace RaceBoard.Business.Managers
{
    public class CoachManager : AbstractManager, ICoachManager
    {
        private readonly ICoachRepository _coachRepository;
        private readonly ICustomValidator<Coach> _coachValidator;
        private readonly IDateTimeHelper _dateTimeHelper;

        #region Constructors

        public CoachManager
            (
                ICoachRepository coachRepository,
                ICustomValidator<Coach> coachValidator,
                IRequestContextManager requestContextManager,
                IDateTimeHelper dateTimeHelper,
                ITranslator translator
            ) : base(requestContextManager, translator)
        {
            _coachRepository = coachRepository;
            _coachValidator = coachValidator;
            _dateTimeHelper = dateTimeHelper;
        }

        #endregion

        #region CoachManager implementation

        public PaginatedResult<Coach> Get(CoachSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _coachRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public Coach Get(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new CoachSearchFilter() { Ids = new int[] { id } };

            var coachs = _coachRepository.Get(searchFilter: searchFilter, paginationFilter: null, sorting: null, context);

            var coach = coachs.Results.FirstOrDefault();
            if (coach == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            return coach;
        }

        public void Create(Coach coach, ITransactionalContext? context = null)
        {
            _coachValidator.SetTransactionalContext(context);

            if (!_coachValidator.IsValid(coach, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _coachValidator.Errors);

            if (context == null)
                context = _coachRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _coachRepository.Create(coach, context);

                _coachRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _coachRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Delete(int id, ITransactionalContext? context = null)
        {
            var coach = this.Get(id, context);

            _coachValidator.SetTransactionalContext(context);

            if (!_coachValidator.IsValid(coach, Scenario.Delete))
                throw new FunctionalException(ErrorType.ValidationError, _coachValidator.Errors);

            if (context == null)
                context = _coachRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _coachRepository.Delete(id, context);

                _coachRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _coachRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        #endregion
    }
}