using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Data;
using RaceBoard.Translations.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Common.Enums;
using RaceBoard.Common.Exceptions;
using RaceBoard.Business.Validators.Interfaces;
using RaceBoard.Common.Helpers.Pagination;

namespace RaceBoard.Business.Managers
{
    public class ContestantManager : AbstractManager, IContestantManager
    {
        private readonly IContestantRepository _contestantRepository;
        private readonly ICustomValidator<Contestant> _contestantValidator;

        #region Constructors

        public ContestantManager
            (
                IContestantRepository contestantRepository,
                ICustomValidator<Contestant> contestantValidator,
                ITranslator translator
            ) : base(translator)
        {
            _contestantRepository = contestantRepository;
            _contestantValidator = contestantValidator;
        }

        #endregion

        #region IContestantManager implementation

        public PaginatedResult<Contestant> Get(ContestantSearchFilter searchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null)
        {
            return _contestantRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public Contestant Get(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new ContestantSearchFilter() { Ids = new int[] { id } };

            var contestants = _contestantRepository.Get(searchFilter: searchFilter, paginationFilter: null, sorting: null, context);

            var contestant = contestants.Results.FirstOrDefault();
            if (contestant == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            return contestant;
        }

        public void Create(Contestant contestant, ITransactionalContext? context = null)
        {
            _contestantValidator.SetTransactionalContext(context);

            if (!_contestantValidator.IsValid(contestant, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _contestantValidator.Errors);

            if (context == null)
                context = _contestantRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _contestantRepository.Create(contestant, context);

                _contestantRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _contestantRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Update(Contestant contestant, ITransactionalContext? context = null)
        {
            _contestantValidator.SetTransactionalContext(context);

            if (!_contestantValidator.IsValid(contestant, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _contestantValidator.Errors);

            if (context == null)
                context = _contestantRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _contestantRepository.Update(contestant, context);

                _contestantRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _contestantRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Delete(int id, ITransactionalContext? context = null)
        {
            var contestant = this.Get(id, context);

            _contestantValidator.SetTransactionalContext(context);

            if (!_contestantValidator.IsValid(contestant, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _contestantValidator.Errors);

            if (context == null)
                context = _contestantRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _contestantRepository.Delete(id, context);

                _contestantRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _contestantRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        #endregion
    }
}