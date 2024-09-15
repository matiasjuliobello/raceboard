using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Business.Validators;
using RaceBoard.Business.Validators.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Common.Exceptions;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Data.Repositories;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Business.Managers
{
    public class CompetitionNewsUpdateManager : AbstractManager, ICompetitionNewsUpdateManager
    {
        private readonly ICompetitionNewsUpdateRepository _competitionNewsUpdateRepository;
        private readonly ICustomValidator<CompetitionNewsUpdate> _competitionNewsUpdateValidator;
        private readonly IDateTimeHelper _dateTimeHelper;

        #region Constructors

        public CompetitionNewsUpdateManager
            (
                ICompetitionNewsUpdateRepository competitionNewsUpdateRepository,
                ICustomValidator<CompetitionNewsUpdate> competitionNewsUpdateValidator,
                ITranslator translator,
                IDateTimeHelper dateTimeHelper
            ) : base(translator)
        {
            _competitionNewsUpdateRepository = competitionNewsUpdateRepository;
            _competitionNewsUpdateValidator = competitionNewsUpdateValidator;
            _dateTimeHelper = dateTimeHelper;
        }

        #endregion

        #region ICompetitionManager implementation

        public PaginatedResult<CompetitionNewsUpdate> Get(CompetitionNewsUpdateSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _competitionNewsUpdateRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public void Create(CompetitionNewsUpdate competitionNewsUpdate, ITransactionalContext? context = null)
        {
            competitionNewsUpdate.Timestamp = _dateTimeHelper.GetCurrentTimestamp();

            _competitionNewsUpdateValidator.SetTransactionalContext(context);

            if (!_competitionNewsUpdateValidator.IsValid(competitionNewsUpdate, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _competitionNewsUpdateValidator.Errors);

            if (context == null)
                context = _competitionNewsUpdateRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _competitionNewsUpdateRepository.Create(competitionNewsUpdate, context);

                _competitionNewsUpdateRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _competitionNewsUpdateRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        #endregion
    }
}