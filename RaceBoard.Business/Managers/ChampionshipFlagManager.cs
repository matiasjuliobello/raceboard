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
    public class ChampionshipFlagManager : AbstractManager, IChampionshipFlagManager
    {
        private readonly IChampionshipFlagRepository _championshipFlagRepository;
        private readonly ICustomValidator<ChampionshipFlag> _championshipFlagValidator;
        private readonly IAuthorizationManager _authorizationManager;
        private readonly IDateTimeHelper _dateTimeHelper;

        public ChampionshipFlagManager
        (
            IChampionshipFlagRepository championshipFlagRepository,
            ICustomValidator<ChampionshipFlag> championshipFlagValidator,
            IRequestContextManager requestContextManager,
            IAuthorizationManager authorizationManager,
            IDateTimeHelper dateTimeHelper,
            ITranslator translator
        ) : base(requestContextManager, translator)
        {
            _championshipFlagRepository = championshipFlagRepository;
            _championshipFlagValidator = championshipFlagValidator;
            _authorizationManager = authorizationManager;
            _dateTimeHelper = dateTimeHelper;
        }

        public PaginatedResult<ChampionshipFlagGroup> GetFlags(ChampionshipFlagSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _championshipFlagRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public ChampionshipFlagGroup Get(int id, ITransactionalContext? context = null)
        {
            //var contextUser = base.GetContextUser();
            //_authorizationManager.ValidatePermission(Domain.Enums.Action.ChampionshipFlag_Get, id, contextUser.Id);

            var searchFilter = new ChampionshipFlagSearchFilter() { Ids = new int[] { id } };

            var championshipFlags = _championshipFlagRepository.Get(searchFilter: searchFilter, paginationFilter: null, sorting: null, context);

            var competitonFlag = championshipFlags.Results.FirstOrDefault();
            if (competitonFlag == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            return competitonFlag;
        }

        public void RaiseFlags(ChampionshipFlagGroup championshipFlagGroup, ITransactionalContext? context = null)
        {
            var contextUser = base.GetContextUser();
            _authorizationManager.ValidatePermission(contextUser.Id, Domain.Enums.Action.ChampionshipFlag_Create, championshipFlagGroup.Championship.Id);

            var currentTime = _dateTimeHelper.GetCurrentTimestamp();

            int? hoursToLower = championshipFlagGroup.Flags[0].HoursToLower;
            int? minuteToLower = championshipFlagGroup.Flags[0].MinutesToLower;

            foreach (var championshipFlag in championshipFlagGroup.Flags)
            {
                championshipFlag.User = new User() { Id = contextUser.Id };
                championshipFlag.Raising = currentTime;

                if (hoursToLower != null)
                {
                    if (championshipFlag.Lowering == null)
                        championshipFlag.Lowering = championshipFlag.Raising;

                    championshipFlag.Lowering = championshipFlag.Lowering.Value.AddHours(hoursToLower.Value);
                }
                if (minuteToLower != null)
                {
                    if (championshipFlag.Lowering == null)
                        championshipFlag.Lowering = championshipFlag.Raising;

                    championshipFlag.Lowering = championshipFlag.Lowering.Value.AddMinutes(minuteToLower.Value);
                }
            }

            //_championshipValidator.SetTransactionalContext(context);

            //if (!_championshipValidator.IsValid(championshipFlagGroup, Scenario.Create))
            //    throw new FunctionalException(ErrorType.ValidationError, _championshipValidator.Errors);

            if (context == null)
                context = _championshipFlagRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _championshipFlagRepository.CreateGroup(championshipFlagGroup, context);

                championshipFlagGroup.Flags.ForEach(x => x.Group = championshipFlagGroup);

                _championshipFlagRepository.AddFlags(championshipFlagGroup.Flags, context);

                context.Confirm();
            }
            catch (Exception)
            {
                if (context != null)
                    context.Cancel();

                throw;
            }
        }

        public void LowerFlags(ChampionshipFlagGroup championshipFlagGroup, ITransactionalContext? context = null)
        {
            var contextUser = base.GetContextUser();
            _authorizationManager.ValidatePermission(contextUser.Id, Domain.Enums.Action.ChampionshipFlag_Update, championshipFlagGroup.Championship.Id);

            var currentTime = _dateTimeHelper.GetCurrentTimestamp();

            foreach (var championshipFlag in championshipFlagGroup.Flags)
            {
                championshipFlag.User = new User() { Id = contextUser.Id };
                championshipFlag.Lowering = currentTime;
            }

            //_championshipValidator.SetTransactionalContext(context);

            //if (!_championshipValidator.IsValid(championshipFlagGroup, Scenario.Update))
            //    throw new FunctionalException(ErrorType.ValidationError, _championshipValidator.Errors);

            if (context == null)
                context = _championshipFlagRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _championshipFlagRepository.UpdateFlags(championshipFlagGroup.Flags, context);

                context.Confirm();
            }
            catch (Exception)
            {
                if (context != null)
                    context.Cancel();

                throw;
            }
        }

        public void RemoveFlags(int idGroup, ITransactionalContext? context = null)
        {
            //var championshipFlag = this.Get(idGroup, context);

            //var contextUser = base.GetContextUser();

            //_authorizationManager.ValidatePermission(Domain.Enums.Action.ChampionshipFlag_Delete, championshipFlag.Championship.Id, contextUser.Id);


            //var filter = new ChampionshipFlagSearchFilter()
            //{
            //    Ids = new int[] { idGroup }
            //};
            //var championshipFlags = this.GetFlags(searchFilter: filter, context: context);

            //if (championshipFlags.Results.Count() == 0)
            //    throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            ////_championshipValidator.SetTransactionalContext(context);

            ////if (!_championshipValidator.IsValid(championship, Scenario.Delete))
            ////    throw new FunctionalException(ErrorType.ValidationError, _championshipValidator.Errors);

            //if (context == null)
            //    context = _championshipFlagRepository.GetTransactionalContext(TransactionContextScope.Internal);

            //try
            //{
            //    _championshipFlagRepository.Delete(idGroup, context);

            //    context.Confirm();
            //}
            //catch (Exception)
            //{
            //    if (context != null)
            //        context.Cancel();

            //    throw;
            //}
        }
    }
}
