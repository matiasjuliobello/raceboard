using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Data;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Data.Repositories;

namespace RaceBoard.Business.Managers
{
    public class CompetitionManager : AbstractManager, ICompetitionManager
    {
        private readonly ICompetitionRepository _competitionRepository;

        #region Constructors

        public CompetitionManager
            (
                ICompetitionRepository competitionRepository,
                ITranslator translator
            ) : base(translator)
        {
            _competitionRepository = competitionRepository;
        }

        #endregion

        #region ICompetitionManager implementation

        public void Create(Competition competition, ITransactionalContext? context = null)
        {
            //_competitionValidator.SetTransactionalContext(context);
            //if (!_competitionValidator.IsValid(competition, Scenario.Create))
            //    throw new FunctionalException(ErrorType.ValidationError, _competitionValidator.Errors);

            if (context == null)
                context = _competitionRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _competitionRepository.Create(competition, context);
                _competitionRepository.SetOrganizations(competition.Id, competition.Organizations, context);

                _competitionRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _competitionRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Update(Competition competition, ITransactionalContext? context = null)
        {
            //_competitionValidator.SetTransactionalContext(context);
            //if (!_competitionValidator.IsValid(competition, Scenario.Create))
            //    throw new FunctionalException(ErrorType.ValidationError, _competitionValidator.Errors);

            if (context == null)
                context = _competitionRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _competitionRepository.Update(competition, context);
                _competitionRepository.SetOrganizations(competition.Id, competition.Organizations, context);

                _competitionRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _competitionRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Delete(int id, ITransactionalContext? context = null)
        {
            //_competitionValidator.SetTransactionalContext(context);
            //if (!_competitionValidator.IsValid(competition, Scenario.Create))
            //    throw new FunctionalException(ErrorType.ValidationError, _competitionValidator.Errors);

            if (context == null)
                context = _competitionRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _competitionRepository.DeleteOrganizations(id, context);
                _competitionRepository.Delete(id, context);

                _competitionRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _competitionRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        #endregion
    }
}