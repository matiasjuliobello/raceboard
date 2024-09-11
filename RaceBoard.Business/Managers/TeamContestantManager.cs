using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Business.Validators.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Common.Exceptions;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Business.Managers
{
    public class TeamContestantManager : AbstractManager, ITeamContestantManager
    {
        private readonly ITeamContestantRepository _teamContestantRepository;
        private readonly ICustomValidator<TeamContestant> _teamContestantValidator;

        #region Constructors

        public TeamContestantManager
            (
                ITeamContestantRepository teamContestantRepository,
                ICustomValidator<TeamContestant> teamContestantValidator,
                ITranslator translator
            ) : base(translator)
        {
            _teamContestantRepository = teamContestantRepository;
            _teamContestantValidator = teamContestantValidator;
        }

        #endregion

        #region ITeamContestantManager implementation

        public PaginatedResult<TeamContestant> Get(TeamContestantSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _teamContestantRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public TeamContestant Get(int id, ITransactionalContext? context = null)
        {
            var teamContestant = _teamContestantRepository.Get(id, context);
            if (teamContestant == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            return teamContestant;
        }

        public void Create(TeamContestant teamContestant, ITransactionalContext? context = null)
        {
            _teamContestantValidator.SetTransactionalContext(context);

            if (!_teamContestantValidator.IsValid(teamContestant, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _teamContestantValidator.Errors);

            if (context == null)
                context = _teamContestantRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _teamContestantRepository.Create(teamContestant, context);

                _teamContestantRepository.ConfirmTransactionalContext(context);
            }
            catch (Exception)
            {
                _teamContestantRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Update(TeamContestant teamContestant, ITransactionalContext? context = null)
        {
            _teamContestantValidator.SetTransactionalContext(context);

            if (!_teamContestantValidator.IsValid(teamContestant, Scenario.Update))
                throw new FunctionalException(ErrorType.ValidationError, _teamContestantValidator.Errors);

            if (context == null)
                context = _teamContestantRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _teamContestantRepository.Update(teamContestant, context);

                _teamContestantRepository.ConfirmTransactionalContext(context);
            }
            catch (Exception)
            {
                _teamContestantRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Delete(int id, ITransactionalContext? context = null)
        {
            var teamContestant = this.Get(id, context);

            _teamContestantValidator.SetTransactionalContext(context);

            if (!_teamContestantValidator.IsValid(teamContestant, Scenario.Delete))
                throw new FunctionalException(ErrorType.ValidationError, _teamContestantValidator.Errors);

            if (context == null)
                context = _teamContestantRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _teamContestantRepository.Delete(teamContestant, context);

                _teamContestantRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _teamContestantRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        #endregion
    }
}
