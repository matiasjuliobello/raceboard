using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Business.Validators.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers
{
    public class CompetitionFlagManager : ICompetitionFlagManager
    {
        private readonly ICompetitionFlagRepository _competitionFlagRepository;

        private readonly ICustomValidator<CompetitionFlag> _competitionFlagValidator;

        public CompetitionFlagManager
        (
            ICompetitionFlagRepository competitionFlagRepository,
            ICustomValidator<CompetitionFlag> competitionFlagValidator
        )
        {
            _competitionFlagRepository = competitionFlagRepository;
            _competitionFlagValidator = competitionFlagValidator;
        }

        public PaginatedResult<CompetitionFlagGroup> GetFlags(CompetitionFlagSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _competitionFlagRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public void RaiseFlags(CompetitionFlagGroup competitionFlagGroup, ITransactionalContext? context = null)
        {
            //_competitionValidator.SetTransactionalContext(context);

            //if (!_competitionValidator.IsValid(competitionFlagGroup, Scenario.Create))
            //    throw new FunctionalException(ErrorType.ValidationError, _competitionValidator.Errors);

            if (context == null)
                context = _competitionFlagRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _competitionFlagRepository.CreateGroup(competitionFlagGroup, context);

                competitionFlagGroup.Flags.ForEach(x => x.Group = competitionFlagGroup);

                _competitionFlagRepository.AddFlags(competitionFlagGroup.Flags, context);

                context.Confirm();
            }
            catch (Exception)
            {
                if (context != null)
                    context.Cancel();

                throw;
            }
        }

        public void LowerFlags(CompetitionFlagGroup competitionFlagGroup, ITransactionalContext? context = null)
        {
            //_competitionValidator.SetTransactionalContext(context);

            //if (!_competitionValidator.IsValid(competitionFlagGroup, Scenario.Update))
            //    throw new FunctionalException(ErrorType.ValidationError, _competitionValidator.Errors);

            if (context == null)
                context = _competitionFlagRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _competitionFlagRepository.UpdateFlags(competitionFlagGroup.Flags, context);

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
            //var filter = new CompetitionFlagSearchFilter()
            //{
            //    Ids = new int[] { idGroup }
            //};
            //var competitionFlags = this.GetFlags(searchFilter: filter, context: context);

            //if (competitionFlags.Results.Count() == 0)
            //    throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            ////_competitionValidator.SetTransactionalContext(context);

            ////if (!_competitionValidator.IsValid(competition, Scenario.Delete))
            ////    throw new FunctionalException(ErrorType.ValidationError, _competitionValidator.Errors);

            //if (context == null)
            //    context = _competitionFlagRepository.GetTransactionalContext(TransactionContextScope.Internal);

            //try
            //{
            //    _competitionFlagRepository.Delete(idGroup, context);

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
