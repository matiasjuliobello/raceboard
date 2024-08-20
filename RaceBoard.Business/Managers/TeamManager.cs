using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Data;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers
{
    public class TeamManager : AbstractManager, ITeamManager
    {
        private readonly ITeamRepository _teamRepository;

        #region Constructors

        public TeamManager
            (
                ITeamRepository teamRepository,
                ITranslator translator
            ) : base(translator)
        {
            _teamRepository = teamRepository;
        }

        #endregion

        #region ITeamManager implementation

        public void Create(Team team, ITransactionalContext? context = null)
        {
            //_teamValidator.SetTransactionalContext(context);
            //if (!_teamValidator.IsValid(team, Scenario.Create))
            //    throw new FunctionalException(ErrorType.ValidationError, _teamValidator.Errors);

            if (context == null)
                context = _teamRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _teamRepository.Create(team, context);
                
                _teamRepository.ConfirmTransactionalContext(context);
            }
            catch (Exception)
            {
                _teamRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Update(Team team, ITransactionalContext? context = null)
        {
            //_teamValidator.SetTransactionalContext(context);
            //if (!_teamValidator.IsValid(team, Scenario.Create))
            //    throw new FunctionalException(ErrorType.ValidationError, _teamValidator.Errors);

            if (context == null)
                context = _teamRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _teamRepository.Update(team, context);

                _teamRepository.ConfirmTransactionalContext(context);
            }
            catch (Exception)
            {
                _teamRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Delete(int id, ITransactionalContext? context = null)
        {
            //_teamValidator.SetTransactionalContext(context);
            //if (!_teamValidator.IsValid(team, Scenario.Create))
            //    throw new FunctionalException(ErrorType.ValidationError, _teamValidator.Errors);

            if (context == null)
                context = _teamRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _teamRepository.Delete(id, context);

                _teamRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _teamRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void SetBoat(TeamBoat teamBoat, ITransactionalContext? context = null)
        {
            //_teamValidator.SetTransactionalContext(context);
            //if (!_teamValidator.IsValid(team, Scenario.Create))
            //    throw new FunctionalException(ErrorType.ValidationError, _teamValidator.Errors);

            if (context == null)
                context = _teamRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _teamRepository.SetBoat(teamBoat, context);

                _teamRepository.ConfirmTransactionalContext(context);
            }
            catch (Exception)
            {
                _teamRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void SetContestants(List<TeamContestant> teamContestants, ITransactionalContext? context = null)
        {
            //_teamValidator.SetTransactionalContext(context);
            //if (!_teamValidator.IsValid(team, Scenario.Create))
            //    throw new FunctionalException(ErrorType.ValidationError, _teamValidator.Errors);

            if (context == null)
                context = _teamRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _teamRepository.SetContestants(teamContestants, context);

                _teamRepository.ConfirmTransactionalContext(context);
            }
            catch (Exception)
            {
                _teamRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        #endregion
    }
}