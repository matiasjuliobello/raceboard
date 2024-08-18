using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Data;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers
{
    public class RaceManager : AbstractManager, IRaceManager
    {
        private readonly IRaceRepository _raceRepository;

        #region Constructors

        public RaceManager
            (
                IRaceRepository raceRepository,
                ITranslator translator
            ) : base(translator)
        {
            _raceRepository = raceRepository;
        }

        #endregion

        #region IRaceManager implementation

        public void Create(Race race, ITransactionalContext? context = null)
        {
            //_raceValidator.SetTransactionalContext(context);
            //if (!_raceValidator.IsValid(race, Scenario.Create))
            //    throw new FunctionalException(ErrorType.ValidationError, _raceValidator.Errors);

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
            //_raceValidator.SetTransactionalContext(context);
            //if (!_raceValidator.IsValid(race, Scenario.Create))
            //    throw new FunctionalException(ErrorType.ValidationError, _raceValidator.Errors);

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
            //_raceValidator.SetTransactionalContext(context);
            //if (!_raceValidator.IsValid(race, Scenario.Create))
            //    throw new FunctionalException(ErrorType.ValidationError, _raceValidator.Errors);

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

        #endregion
    }
}