using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Data;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers
{
    public class ContestantRoleManager : AbstractManager, IContestantRoleManager
    {
        private readonly IContestantRoleRepository _contestantRoleRepository;

        #region Constructors

        public ContestantRoleManager
            (
                IContestantRoleRepository contestantRoleRepository,
                ITranslator translator
            ) : base(translator)
        {
            _contestantRoleRepository = contestantRoleRepository;
        }

        #endregion

        #region IContestantRoleManager implementation

        public void Create(ContestantRole contestantRole, ITransactionalContext? context = null)
        {
            //_contestantRoleValidator.SetTransactionalContext(context);
            //if (!_contestantRoleValidator.IsValid(contestantRole, Scenario.Create))
            //    throw new FunctionalException(ErrorType.ValidationError, _contestantRoleValidator.Errors);

            if (context == null)
                context = _contestantRoleRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _contestantRoleRepository.Create(contestantRole, context);

                _contestantRoleRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _contestantRoleRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Update(ContestantRole contestantRole, ITransactionalContext? context = null)
        {
            //_contestantRoleValidator.SetTransactionalContext(context);
            //if (!_contestantRoleValidator.IsValid(contestantRole, Scenario.Create))
            //    throw new FunctionalException(ErrorType.ValidationError, _contestantRoleValidator.Errors);

            if (context == null)
                context = _contestantRoleRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _contestantRoleRepository.Update(contestantRole, context);

                _contestantRoleRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _contestantRoleRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Delete(int id, ITransactionalContext? context = null)
        {
            //_contestantRoleValidator.SetTransactionalContext(context);
            //if (!_contestantRoleValidator.IsValid(contestantRole, Scenario.Create))
            //    throw new FunctionalException(ErrorType.ValidationError, _contestantRoleValidator.Errors);

            if (context == null)
                context = _contestantRoleRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _contestantRoleRepository.Delete(id, context);

                _contestantRoleRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _contestantRoleRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        #endregion
    }
}