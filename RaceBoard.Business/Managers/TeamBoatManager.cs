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
    public class TeamBoatManager : AbstractManager, ITeamBoatManager
    {
        private readonly ITeamBoatRepository _teamBoatRepository;
        private readonly ICustomValidator<TeamBoat> _teamBoatValidator;

        #region Constructors

        public TeamBoatManager
            (
                ITeamBoatRepository teamBoatRepository,
                ICustomValidator<TeamBoat> teamBoatValidator,
                ITranslator translator
            ) : base(translator)
        {
            _teamBoatRepository = teamBoatRepository;
            _teamBoatValidator = teamBoatValidator;
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

            _teamBoatValidator.SetTransactionalContext(context);

            if (!_teamBoatValidator.IsValid(teamBoat, Scenario.Delete))
                throw new FunctionalException(ErrorType.ValidationError, _teamBoatValidator.Errors);

            if (context == null)
                context = _teamBoatRepository.GetTransactionalContext(TransactionContextScope.Internal);

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
