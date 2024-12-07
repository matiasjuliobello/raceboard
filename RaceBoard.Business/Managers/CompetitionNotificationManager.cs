using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Business.Validators.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Common.Exceptions;
using RaceBoard.Common.Helpers;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Data.Repositories;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Business.Managers
{
    public class CompetitionNotificationManager : AbstractManager, ICompetitionNotificationManager
    {
        private readonly ICompetitionNotificationRepository _competitionNotificationRepository;
        private readonly ICustomValidator<CompetitionNotification> _competitionNotificationValidator;
        private readonly IDateTimeHelper _dateTimeHelper;

        #region Constructors

        public CompetitionNotificationManager
            (
                ICompetitionNotificationRepository competitionNotificationRepository,
                ICustomValidator<CompetitionNotification> competitionNotificationValidator,
                ITranslator translator,
                IDateTimeHelper dateTimeHelper
            ) : base(translator)
        {
            _competitionNotificationRepository = competitionNotificationRepository;
            _competitionNotificationValidator = competitionNotificationValidator;
            _dateTimeHelper = dateTimeHelper;
        }

        #endregion

        #region ICompetitionNotificationManager implementation

        public PaginatedResult<CompetitionNotification> Get(CompetitionNotificationSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _competitionNotificationRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public void Create(CompetitionNotification competitionNotification, ITransactionalContext? context = null)
        {
            competitionNotification.Timestamp = _dateTimeHelper.GetCurrentTimestamp();
            
            _competitionNotificationValidator.SetTransactionalContext(context);

            if (!_competitionNotificationValidator.IsValid(competitionNotification, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _competitionNotificationValidator.Errors);

            if (context == null)
                context = _competitionNotificationRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _competitionNotificationRepository.Create(competitionNotification, context);
                _competitionNotificationRepository.AssociateRaceClasses(competitionNotification, context);

                _competitionNotificationRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _competitionNotificationRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        #endregion
    }
}