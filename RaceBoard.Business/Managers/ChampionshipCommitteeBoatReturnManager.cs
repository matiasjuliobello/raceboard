using RaceBoard.Business.Helpers.Interfaces;
using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Business.Validators.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Common.Exceptions;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Business.Managers
{
    public class ChampionshipCommitteeBoatReturnManager : AbstractManager, IChampionshipCommitteeBoatReturnManager
    {
        private readonly IChampionshipCommitteeBoatReturnRepository _committeeBoatReturnRepository;
        private readonly IUserAccessRepository _userAccessRepository;

        private readonly ICustomValidator<ChampionshipCommitteeBoatReturn> _committeeBoatReturnValidator;
        private readonly IDateTimeHelper _dateTimeHelper;

        private readonly IAuthorizationManager _authorizationManager;

        #region Constructors

        public ChampionshipCommitteeBoatReturnManager
            (
                IChampionshipCommitteeBoatReturnRepository committeeBoatReturnRepository,
                IUserAccessRepository userAccessRepository,
                ICustomValidator<ChampionshipCommitteeBoatReturn> committeeBoatReturnValidator,
                IDateTimeHelper dateTimeHelper,
                IRequestContextManager requestContextManager,
                ITranslator translator,
                IAuthorizationManager authorizationManager
            ) : base(requestContextManager, translator)
        {
            _committeeBoatReturnRepository = committeeBoatReturnRepository;
            _userAccessRepository = userAccessRepository;
            _committeeBoatReturnValidator = committeeBoatReturnValidator;
            _dateTimeHelper = dateTimeHelper;
            _authorizationManager = authorizationManager;
        }

        #endregion


        public PaginatedResult<ChampionshipCommitteeBoatReturn> Get(ChampionshipCommitteeBoatReturnSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _committeeBoatReturnRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public ChampionshipCommitteeBoatReturn Get(int id, ITransactionalContext? context = null)
        {
            //var contextUser = base.GetContextUser();
            //_authorizationManager.ValidatePermission(Domain.Enums.Action.ChampionshipFile_Get, id, contextUser.Id);

            var searchFilter = new ChampionshipCommitteeBoatReturnSearchFilter()
            {
                Ids = new int[] { id }
            };

            var committeeBoatReturns = _committeeBoatReturnRepository.Get(searchFilter: searchFilter, paginationFilter: null, sorting: null, context);

            var committeeBoatReturn = committeeBoatReturns.Results.FirstOrDefault();
            if (committeeBoatReturn == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            return committeeBoatReturn;
        }

        public void Create(ChampionshipCommitteeBoatReturn committeeBoatReturn, ITransactionalContext? context = null)
        {
            committeeBoatReturn.ReturnTime = _dateTimeHelper.GetCurrentTimestamp();

            _committeeBoatReturnValidator.SetTransactionalContext(context);

            if (!_committeeBoatReturnValidator.IsValid(committeeBoatReturn, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _committeeBoatReturnValidator.Errors);

            if (context == null)
                context = _committeeBoatReturnRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _committeeBoatReturnRepository.Create(committeeBoatReturn, context);
                _committeeBoatReturnRepository.DeleteRaceClasses(committeeBoatReturn.Id, context);
                _committeeBoatReturnRepository.AssociateRaceClasses(committeeBoatReturn, context);

                context.Confirm();
            }
            catch (Exception)
            {
                if (context != null)
                    context.Cancel();

                throw;
            }
        }

        public void Update(ChampionshipCommitteeBoatReturn championshipCommitteeBoatReturn, ITransactionalContext? context = null)
        {
            _committeeBoatReturnValidator.SetTransactionalContext(context);

            if (!_committeeBoatReturnValidator.IsValid(championshipCommitteeBoatReturn, Scenario.Update))
                throw new FunctionalException(ErrorType.ValidationError, _committeeBoatReturnValidator.Errors);

            if (context == null)
                context = _committeeBoatReturnRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _committeeBoatReturnRepository.Update(championshipCommitteeBoatReturn, context);
                _committeeBoatReturnRepository.DeleteRaceClasses(championshipCommitteeBoatReturn.Id, context);
                _committeeBoatReturnRepository.AssociateRaceClasses(championshipCommitteeBoatReturn, context);

                context.Confirm();
            }
            catch (Exception)
            {
                if (context != null)
                    context.Cancel();

                throw;
            }
        }

        public void Delete(int id, ITransactionalContext? context = null)
        {
            if (context == null)
                context = _committeeBoatReturnRepository.GetTransactionalContext(TransactionContextScope.Internal);

            var searchFilter = new ChampionshipCommitteeBoatReturnSearchFilter() { Ids = new int[] { id } };
            var committeeBoatReturn = this.Get(searchFilter, paginationFilter: null, sorting: null, context);

            //_committeeBoatReturnValidator.SetTransactionalContext(context);

            //if (!_committeeBoatReturnValidator.IsValid(committeeBoatReturn, Scenario.Delete))
            //    throw new FunctionalException(ErrorType.ValidationError, _committeeBoatReturnValidator.Errors);

            if (context == null)
                context = _committeeBoatReturnRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _committeeBoatReturnRepository.DeleteRaceClasses(id, context);
                _committeeBoatReturnRepository.Delete(id, context);

                context.Confirm();
            }
            catch (Exception)
            {
                if (context != null)
                    context.Cancel();

                throw;
            }
        }

    }
}
