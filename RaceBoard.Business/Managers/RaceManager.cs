using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Data;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Common.Enums;
using RaceBoard.Common.Exceptions;
using RaceBoard.Business.Validators.Interfaces;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Common.Helpers;

namespace RaceBoard.Business.Managers
{
    public class RaceManager : AbstractManager, IRaceManager
    {
        private readonly IRaceRepository _raceRepository;
        private readonly ICustomValidator<Race> _raceValidator;
        private readonly ICustomValidator<RaceComplaint> _raceComplaintValidator;
        private readonly IDateTimeHelper _dateTimeHelper;

        #region Constructors

        public RaceManager
            (
                IRaceRepository raceRepository,
                ICustomValidator<Race> raceValidator,
                ICustomValidator<RaceComplaint> raceComplaintValidator,
                IDateTimeHelper dateTimeHelper,
                ITranslator translator
            ) : base(translator)
        {
            _raceRepository = raceRepository;
            _raceValidator = raceValidator;
            _raceComplaintValidator = raceComplaintValidator;
            _dateTimeHelper = dateTimeHelper;
        }

        #endregion

        #region IRaceManager implementation

        public PaginatedResult<Race> Get(RaceSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _raceRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public Race Get(int id, ITransactionalContext? context = null)
        {
            var race = _raceRepository.Get(id, context);
            if (race == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            return race;
        }

        public void Create(Race race, ITransactionalContext? context = null)
        {
            _raceValidator.SetTransactionalContext(context);

            if (!_raceValidator.IsValid(race, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _raceValidator.Errors);

            if (context == null)
                context = _raceRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _raceRepository.Create(race, context);

                _raceRepository.ConfirmTransactionalContext(context);
            }
            catch (Exception)
            {
                _raceRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Update(Race race, ITransactionalContext? context = null)
        {
            _raceValidator.SetTransactionalContext(context);

            if (!_raceValidator.IsValid(race, Scenario.Update))
                throw new FunctionalException(ErrorType.ValidationError, _raceValidator.Errors);

            if (context == null)
                context = _raceRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _raceRepository.Update(race, context);

                _raceRepository.ConfirmTransactionalContext(context);
            }
            catch (Exception)
            {
                _raceRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Delete(int id, ITransactionalContext? context = null)
        {
            var race = this.Get(id, context);

            _raceValidator.SetTransactionalContext(context);

            if (!_raceValidator.IsValid(race, Scenario.Delete))
                throw new FunctionalException(ErrorType.ValidationError, _raceValidator.Errors);

            if (context == null)
                context = _raceRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _raceRepository.Delete(id, context);

                _raceRepository.ConfirmTransactionalContext(context);
            }
            catch (Exception)
            {
                _raceRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void CreateComplaint(RaceComplaint raceComplaint, ITransactionalContext? context = null)
        {
            raceComplaint.Timestamp = _dateTimeHelper.GetCurrentTimestamp();

            _raceComplaintValidator.SetTransactionalContext(context);

            if (!_raceComplaintValidator.IsValid(raceComplaint, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _raceComplaintValidator.Errors);

            if (context == null)
                context = _raceRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _raceRepository.CreateComplaint(raceComplaint, context);

                _raceRepository.ConfirmTransactionalContext(context);
            }
            catch (Exception)
            {
                _raceRepository.CancelTransactionalContext(context);
                throw;
            }
        }


        #endregion
    }
}