using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Data;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers
{
    public class BoatManager : AbstractManager, IBoatManager
    {
        private readonly IBoatRepository _boatRepository;

        #region Constructors

        public BoatManager
            (
                IBoatRepository boatRepository,
                ITranslator translator
            ) : base(translator)
        {
            _boatRepository = boatRepository;
        }

        #endregion

        #region IBoatManager implementation

        public void Create(Boat boat, ITransactionalContext? context = null)
        {
            //_boatValidator.SetTransactionalContext(context);
            //if (!_boatValidator.IsValid(boat, Scenario.Create))
            //    throw new FunctionalException(ErrorType.ValidationError, _boatValidator.Errors);

            if (context == null)
                context = _boatRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _boatRepository.Create(boat, context);

                _boatRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _boatRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Update(Boat boat, ITransactionalContext? context = null)
        {
            //_boatValidator.SetTransactionalContext(context);
            //if (!_boatValidator.IsValid(boat, Scenario.Create))
            //    throw new FunctionalException(ErrorType.ValidationError, _boatValidator.Errors);

            if (context == null)
                context = _boatRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _boatRepository.Update(boat, context);

                _boatRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _boatRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Delete(int id, ITransactionalContext? context = null)
        {
            //_boatValidator.SetTransactionalContext(context);
            //if (!_boatValidator.IsValid(boat, Scenario.Create))
            //    throw new FunctionalException(ErrorType.ValidationError, _boatValidator.Errors);

            if (context == null)
                context = _boatRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _boatRepository.Delete(id, context);

                _boatRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _boatRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        #endregion
    }
}