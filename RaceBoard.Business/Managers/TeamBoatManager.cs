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
using Enums = RaceBoard.Domain.Enums;

namespace RaceBoard.Business.Managers
{
    public class TeamBoatManager : AbstractManager, ITeamBoatManager
    {
        private readonly ITeamBoatRepository _teamBoatRepository;
        private readonly ICustomValidator<TeamBoat> _teamBoatValidator;
        private readonly IAuthorizationManager _authorizationManager;

        #region Constructors

        public TeamBoatManager
            (
                ITeamBoatRepository teamBoatRepository,
                ICustomValidator<TeamBoat> teamBoatValidator,
                IRequestContextManager requestContextManager,
                IAuthorizationManager authorizationManager,
                ITranslator translator
            ) : base(requestContextManager, translator)
        {
            _teamBoatRepository = teamBoatRepository;
            _teamBoatValidator = teamBoatValidator;
            _authorizationManager = authorizationManager;
        }

        #endregion

        #region ITeamBoatManager implementation

        public PaginatedResult<TeamBoat> Get(TeamBoatSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _teamBoatRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public TeamBoat Get(int id, ITransactionalContext? context = null)
        {
            var teamBoat = _teamBoatRepository.Get(id, context);
            if (teamBoat == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            return teamBoat;
        }

        public void Create(TeamBoat teamBoat, ITransactionalContext? context = null)
        {
            var contextUser = base.GetContextUser();
            _authorizationManager.ValidatePermission(contextUser.Id, Enums.Action.TeamBoat_Create, teamBoat.Team.Id);


            _teamBoatValidator.SetTransactionalContext(context);

            if (!_teamBoatValidator.IsValid(teamBoat, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _teamBoatValidator.Errors);

            if (context == null)
                context = _teamBoatRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _teamBoatRepository.Create(teamBoat, context);

                _teamBoatRepository.ConfirmTransactionalContext(context);
            }
            catch (Exception)
            {
                _teamBoatRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Update(TeamBoat teamBoat, ITransactionalContext? context = null)
        {
            var contextUser = base.GetContextUser();
            _authorizationManager.ValidatePermission(contextUser.Id, Enums.Action.TeamBoat_Update, teamBoat.Team.Id);

            _teamBoatValidator.SetTransactionalContext(context);

            if (!_teamBoatValidator.IsValid(teamBoat, Scenario.Update))
                throw new FunctionalException(ErrorType.ValidationError, _teamBoatValidator.Errors);

            if (context == null)
                context = _teamBoatRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _teamBoatRepository.Update(teamBoat, context);

                _teamBoatRepository.ConfirmTransactionalContext(context);
            }
            catch (Exception)
            {
                _teamBoatRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Delete(int id, ITransactionalContext? context = null)
        {
            var teamBoat = this.Get(id, context);

            var contextUser = base.GetContextUser();
            _authorizationManager.ValidatePermission(contextUser.Id, Enums.Action.TeamBoat_Delete, teamBoat.Team.Id);

            if (context == null)
                context = _teamBoatRepository.GetTransactionalContext(TransactionContextScope.Internal);

            _teamBoatValidator.SetTransactionalContext(context);

            if (!_teamBoatValidator.IsValid(teamBoat, Scenario.Delete))
                throw new FunctionalException(ErrorType.ValidationError, _teamBoatValidator.Errors);

            try
            {
                _teamBoatRepository.Delete(teamBoat, context);

                _teamBoatRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _teamBoatRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        #endregion
    }
}
