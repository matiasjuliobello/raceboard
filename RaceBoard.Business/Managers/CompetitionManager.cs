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

namespace RaceBoard.Business.Managers
{
    public class CompetitionManager : AbstractManager, ICompetitionManager
    {
        private readonly ICompetitionRepository _competitionRepository;
        private readonly ICustomValidator<Competition> _competitionValidator;

        #region Constructors

        public CompetitionManager
            (
                ICompetitionRepository competitionRepository,
                ICustomValidator<Competition> competitionValidator,
                ITranslator translator
            ) : base(translator)
        {
            _competitionRepository = competitionRepository;
            _competitionValidator = competitionValidator;
        }

        #endregion

        #region ICompetitionManager implementation

        public PaginatedResult<Competition> Get(CompetitionSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _competitionRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public Competition Get(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new CompetitionSearchFilter() { Ids = new int[] { id } };

            var competitions = _competitionRepository.Get(searchFilter: searchFilter, paginationFilter: null, sorting: null, context);

            var competiton = competitions.Results.FirstOrDefault();
            if (competiton == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            return competiton;
        }

        public void Create(Competition competition, ITransactionalContext? context = null)
        {
            if (context == null)
                context = _competitionRepository.GetTransactionalContext(TransactionContextScope.Internal);

            _competitionValidator.SetTransactionalContext(context);

            if (!_competitionValidator.IsValid(competition, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _competitionValidator.Errors);

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
            if (context == null)
                context = _competitionRepository.GetTransactionalContext(TransactionContextScope.Internal);

            _competitionValidator.SetTransactionalContext(context);

            if (!_competitionValidator.IsValid(competition, Scenario.Update))
                throw new FunctionalException(ErrorType.ValidationError, _competitionValidator.Errors);

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
            var competition = this.Get(id, context);

            _competitionValidator.SetTransactionalContext(context);

            if (!_competitionValidator.IsValid(competition, Scenario.Delete))
                throw new FunctionalException(ErrorType.ValidationError, _competitionValidator.Errors);

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

        public List<CompetitionRaceClass> GetRaceClasses(int idCompetition, ITransactionalContext? context = null)
        {
            return _competitionRepository.GetRaceClasses(idCompetition, context);
        }

        public void SetRaceClasses(List<CompetitionRaceClass> competitionRaceClasses, ITransactionalContext? context = null)
        {
            if (competitionRaceClasses == null || competitionRaceClasses.Count() == 0)
                return;

            if (context == null)
                context = _competitionRepository.GetTransactionalContext(TransactionContextScope.Internal);

            //_competitionRaceClassValidator.SetTransactionalContext(context);

            //if (!_competitionRaceClassValidator.IsValid(competitionRaceClasses, Scenario.Create))
            //    throw new FunctionalException(ErrorType.ValidationError, _competitionRaceClassValidator.Errors);

            try
            {
                int idCompetition = competitionRaceClasses.First().Competition.Id;

                _competitionRepository.RemoveRaceClasses(idCompetition, context);

                List<RaceClass> raceClasses = competitionRaceClasses
                    .Select(x => x.RaceClass)
                    .ToList();

                _competitionRepository.AddRaceClasses(idCompetition, raceClasses, context);

                _competitionRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _competitionRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public List<CompetitionTerm> GetRegistrationTerms(int idCompetition, ITransactionalContext? context = null)
        {
            return _competitionRepository.GetRegistrationTerms(idCompetition, context);
        }

        public void SetRegistrationTerms(List<CompetitionRegistrationTerm> registrationTerms, ITransactionalContext? context = null)
        {
            if (registrationTerms == null || registrationTerms.Count() == 0)
                return;

            if (context == null)
                context = _competitionRepository.GetTransactionalContext(TransactionContextScope.Internal);

            //_competitionTermValidator.SetTransactionalContext(context);

            //if (!_competitionTermValidator.IsValid(registrationTerms, Scenario.Create))
            //    throw new FunctionalException(ErrorType.ValidationError, _competitionTermValidator.Errors);

            try
            {
                int idCompetition = registrationTerms.First().Competition.Id;

                _competitionRepository.RemoveRegistrationTerms(idCompetition, context);

                _competitionRepository.AddRegistrationTerms(registrationTerms, context);

                _competitionRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _competitionRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public List<CompetitionTerm> GetAccreditationTerms(int idCompetition, ITransactionalContext? context = null)
        {
            return _competitionRepository.GetAccreditationTerms(idCompetition, context);
        }

        public void SetAccreditationTerms(List<CompetitionAccreditationTerm> accreditationTerms, ITransactionalContext? context = null)
        {
            if (accreditationTerms == null || accreditationTerms.Count() == 0)
                return;

            if (context == null)
                context = _competitionRepository.GetTransactionalContext(TransactionContextScope.Internal);

            //_competitionTermValidator.SetTransactionalContext(context);

            //if (!_competitionTermValidator.IsValid(accreditationTerms, Scenario.Create))
            //    throw new FunctionalException(ErrorType.ValidationError, _competitionTermValidator.Errors);

            try
            {
                int idCompetition = accreditationTerms.First().Competition.Id;

                _competitionRepository.RemoveAccreditationTerms(idCompetition, context);

                _competitionRepository.AddAccreditationTerms(accreditationTerms, context);

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