using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Business.Validators;
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
    public class ChampionshipNotificationManager : AbstractManager, IChampionshipNotificationManager
    {
        private readonly IChampionshipNotificationRepository _championshipNotificationRepository;
        private readonly ICustomValidator<ChampionshipNotification> _championshipNotificationValidator;
        private readonly IDateTimeHelper _dateTimeHelper;

        #region Constructors

        public ChampionshipNotificationManager
            (
                IChampionshipNotificationRepository championshipNotificationRepository,
                ICustomValidator<ChampionshipNotification> championshipNotificationValidator,
                ITranslator translator,
                IDateTimeHelper dateTimeHelper
            ) : base(translator)
        {
            _championshipNotificationRepository = championshipNotificationRepository;
            _championshipNotificationValidator = championshipNotificationValidator;
            _dateTimeHelper = dateTimeHelper;
        }

        #endregion

        #region IChampionshipNotificationManager implementation

        public PaginatedResult<ChampionshipNotification> Get(ChampionshipNotificationSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _championshipNotificationRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public ChampionshipNotification Get(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new ChampionshipNotificationSearchFilter() { Ids = new int[] { id } };

            var championships = _championshipNotificationRepository.Get(searchFilter, paginationFilter: null, sorting: null, context);

            var championship = championships.Results.FirstOrDefault();
            if (championship == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            return championship;
        }

        public void Create(ChampionshipNotification championshipNotification, ITransactionalContext? context = null)
        {
            championshipNotification.CreationDate = _dateTimeHelper.GetCurrentTimestamp();
            
            _championshipNotificationValidator.SetTransactionalContext(context);

            if (!_championshipNotificationValidator.IsValid(championshipNotification, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _championshipNotificationValidator.Errors);

            if (context == null)
                context = _championshipNotificationRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _championshipNotificationRepository.Create(championshipNotification, context);
                _championshipNotificationRepository.AssociateRaceClasses(championshipNotification, context);

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
            var championshipNotification = this.Get(id, context);

            _championshipNotificationValidator.SetTransactionalContext(context);

            if (!_championshipNotificationValidator.IsValid(championshipNotification, Scenario.Delete))
                throw new FunctionalException(ErrorType.ValidationError, _championshipNotificationValidator.Errors);

            if (context == null)
                context = _championshipNotificationRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _championshipNotificationRepository.DeleteRaceClasses(id, context);
                _championshipNotificationRepository.Delete(id, context);

                context.Confirm();
            }
            catch (Exception)
            {
                if (context != null)
                    context.Cancel();

                throw;
            }
        }


        #endregion
    }
}