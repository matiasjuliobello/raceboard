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
using RaceBoard.Data.Repositories;

namespace RaceBoard.Business.Managers
{
    public class RaceManager : AbstractManager, IRaceManager
    {
        private readonly IRaceRepository _raceRepository;
        private readonly IRaceProtestRepository _raceProtestRepository;
        private readonly IRaceCommitteeBoatReturnRepository _raceCommitteeBoatReturnRepository;
        private readonly ICustomValidator<Race> _raceValidator;
        private readonly ICustomValidator<RaceProtest> _raceProtestValidator;
        private readonly ICustomValidator<RaceCommitteeBoatReturn> _raceCommitteeBoatReturnValidator;
        private readonly IDateTimeHelper _dateTimeHelper;

        #region Constructors

        public RaceManager
            (
                IRaceRepository raceRepository,
                IRaceProtestRepository raceProtestRepository,
                IRaceCommitteeBoatReturnRepository raceCommitteeBoatReturnRepository,
                ICustomValidator<Race> raceValidator,
                ICustomValidator<RaceProtest> raceProtestValidator,
                ICustomValidator<RaceCommitteeBoatReturn> raceCommitteeBoatReturnValidator,
                IDateTimeHelper dateTimeHelper,
                ITranslator translator
            ) : base(translator)
        {
            _raceRepository = raceRepository;
            _raceProtestRepository = raceProtestRepository;
            _raceCommitteeBoatReturnRepository = raceCommitteeBoatReturnRepository;
            _raceValidator = raceValidator;
            _raceProtestValidator = raceProtestValidator;
            _raceCommitteeBoatReturnValidator = raceCommitteeBoatReturnValidator;
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

        public void CreateProtest(RaceProtest raceProtest, ITransactionalContext? context = null)
        {
            raceProtest.Submission = _dateTimeHelper.GetCurrentTimestamp();

            _raceProtestValidator.SetTransactionalContext(context);

            if (!_raceProtestValidator.IsValid(raceProtest, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _raceProtestValidator.Errors);

            if (context == null)
                context = _raceProtestRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _raceProtestRepository.Create(raceProtest, context);

                _raceProtestRepository.ConfirmTransactionalContext(context);
            }
            catch (Exception)
            {
                _raceProtestRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void CreateCommitteeBoatReturns(RaceCommitteeBoatReturn committeeBoatReturn, ITransactionalContext? context = null)
        {
            committeeBoatReturn.Return = _dateTimeHelper.GetCurrentTimestamp();

            _raceCommitteeBoatReturnValidator.SetTransactionalContext(context);

            if (!_raceCommitteeBoatReturnValidator.IsValid(committeeBoatReturn, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _raceCommitteeBoatReturnValidator.Errors);

            if (context == null)
                context = _raceCommitteeBoatReturnRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _raceCommitteeBoatReturnRepository.Create(committeeBoatReturn, context);

                _raceCommitteeBoatReturnRepository.ConfirmTransactionalContext(context);
            }
            catch (Exception)
            {
                _raceCommitteeBoatReturnRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        #endregion
    }
}