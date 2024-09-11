using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Data;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Common.Exceptions;
using RaceBoard.Common.Enums;
using RaceBoard.Business.Validators.Interfaces;

namespace RaceBoard.Business.Managers
{
    public class TeamManager : AbstractManager, ITeamManager
    {
        private readonly ITeamRepository _teamRepository;
        private readonly ICustomValidator<Team> _teamValidator;

        #region Constructors

        public TeamManager
            (
                ITeamRepository teamRepository,
                ICustomValidator<Team> teamValidator,
                ITranslator translator
            ) : base(translator)
        {
            _teamRepository = teamRepository;
            _teamValidator = teamValidator;
        }

        #endregion

        #region ITeamManager implementation

        public PaginatedResult<Team> Get(TeamSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _teamRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public Team Get(int id, ITransactionalContext? context = null)
        {
            var team = _teamRepository.Get(id, context);
            if (team == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            return team;
        }

        public void Create(Team team, ITransactionalContext? context = null)
        {
            _teamValidator.SetTransactionalContext(context);

            if (!_teamValidator.IsValid(team, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _teamValidator.Errors);

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
            _teamValidator.SetTransactionalContext(context);

            if (!_teamValidator.IsValid(team, Scenario.Update))
                throw new FunctionalException(ErrorType.ValidationError, _teamValidator.Errors);

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
            var team = this.Get(id, context);

            _teamValidator.SetTransactionalContext(context);

            if (!_teamValidator.IsValid(team, Scenario.Delete))
                throw new FunctionalException(ErrorType.ValidationError, _teamValidator.Errors);

            if (context == null)
                context = _teamRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _teamRepository.Delete(team, context);

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