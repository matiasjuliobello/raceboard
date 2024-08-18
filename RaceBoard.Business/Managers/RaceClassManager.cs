using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Data;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers
{
    public class RaceClassManager : AbstractManager, IRaceClassManager
    {
        private readonly IRaceClassRepository _raceClassRepository;

        #region Constructors

        public RaceClassManager
            (
                IRaceClassRepository raceClassRepository,
                ITranslator translator
            ) : base(translator)
        {
            _raceClassRepository = raceClassRepository;
        }

        #endregion

        #region IRaceClassManager implementation

        public void Create(RaceClass raceClass, ITransactionalContext? context = null)
        {
            //_raceClassValidator.SetTransactionalContext(context);
            //if (!_raceClassValidator.IsValid(raceClass, Scenario.Create))
            //    throw new FunctionalException(ErrorType.ValidationError, _raceClassValidator.Errors);

            if (context == null)
                context = _raceClassRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _raceClassRepository.Create(raceClass, context);

                _raceClassRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _raceClassRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Update(RaceClass raceClass, ITransactionalContext? context = null)
        {
            //_raceClassValidator.SetTransactionalContext(context);
            //if (!_raceClassValidator.IsValid(raceClass, Scenario.Create))
            //    throw new FunctionalException(ErrorType.ValidationError, _raceClassValidator.Errors);

            if (context == null)
                context = _raceClassRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _raceClassRepository.Update(raceClass, context);

                _raceClassRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _raceClassRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Delete(int id, ITransactionalContext? context = null)
        {
            //_raceClassValidator.SetTransactionalContext(context);
            //if (!_raceClassValidator.IsValid(raceClass, Scenario.Create))
            //    throw new FunctionalException(ErrorType.ValidationError, _raceClassValidator.Errors);

            if (context == null)
                context = _raceClassRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _raceClassRepository.Delete(id, context);

                _raceClassRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _raceClassRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        #endregion
    }
}