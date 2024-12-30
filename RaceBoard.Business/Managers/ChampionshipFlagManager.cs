using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Business.Validators.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers
{
    public class ChampionshipFlagManager : IChampionshipFlagManager
    {
        private readonly IChampionshipFlagRepository _championshipFlagRepository;

        private readonly ICustomValidator<ChampionshipFlag> _championshipFlagValidator;

        public ChampionshipFlagManager
        (
            IChampionshipFlagRepository championshipFlagRepository,
            ICustomValidator<ChampionshipFlag> championshipFlagValidator
        )
        {
            _championshipFlagRepository = championshipFlagRepository;
            _championshipFlagValidator = championshipFlagValidator;
        }

        public PaginatedResult<ChampionshipFlagGroup> GetFlags(ChampionshipFlagSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _championshipFlagRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public void RaiseFlags(ChampionshipFlagGroup championshipFlagGroup, ITransactionalContext? context = null)
        {
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
