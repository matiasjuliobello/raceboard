using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Data;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers
{
    public class ContestantManager : AbstractManager, IContestantManager
    {
        private readonly IContestantRepository _contestantRepository;

        #region Constructors

        public ContestantManager
            (
                IContestantRepository contestantRepository,
                ITranslator translator
            ) : base(translator)
        {
            _contestantRepository = contestantRepository;
        }

        #endregion

        #region IContestantManager implementation

        public void Create(Contestant contestant, ITransactionalContext? context = null)
        {
            //_contestantValidator.SetTransactionalContext(context);
            //if (!_contestantValidator.IsValid(contestant, Scenario.Create))
            //    throw new FunctionalException(ErrorType.ValidationError, _contestantValidator.Errors);

            if (context == null)
                context = _contestantRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _contestantRepository.Create(contestant, context);

                _contestantRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _contestantRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Update(Contestant contestant, ITransactionalContext? context = null)
        {
            //_contestantValidator.SetTransactionalContext(context);
            //if (!_contestantValidator.IsValid(contestant, Scenario.Update))
            //    throw new FunctionalException(ErrorType.ValidationError, _contestantValidator.Errors);

            if (context == null)
                context = _contestantRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _contestantRepository.Update(contestant, context);

                _contestantRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _contestantRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Delete(int id, ITransactionalContext? context = null)
        {
            //_contestantValidator.SetTransactionalContext(context);
            //if (!_contestantValidator.IsValid(contestant, Scenario.Delete))
            //    throw new FunctionalException(ErrorType.ValidationError, _contestantValidator.Errors);

            if (context == null)
                context = _contestantRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _contestantRepository.Delete(id, context);

                _contestantRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _contestantRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        #endregion
    }
}