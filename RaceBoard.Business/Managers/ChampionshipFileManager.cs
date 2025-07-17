using Microsoft.Extensions.Configuration;
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
using RaceBoard.FileStorage.Interfaces;
using RaceBoard.Translations.Interfaces;


namespace RaceBoard.Business.Managers
{
    public class ChampionshipFileManager : AbstractManager, IChampionshipFileManager
    {
        private readonly IChampionshipFileRepository _championshipFileRepository;
        private readonly ICustomValidator<ChampionshipFile> _championshipFileValidator;

        private readonly IFileHelper _fileHelper;
        private readonly INotificationHelper _notificationHelper;

        private readonly IFileRepository _fileRepository;

        private readonly IAuthorizationManager _authorizationManager;

        #region Constructors

        public ChampionshipFileManager
            (
                IChampionshipFileRepository championshipFileRepository,
                IAuthorizationManager authorizationManager,
                ICustomValidator<ChampionshipFile> championshipFileValidator,
                ITranslator translator,
                IDateTimeHelper dateTimeHelper,
                IFileHelper fileHelper,
                INotificationHelper notificationHelper,
                IFileStorageProvider fileStorageProvider,
                IFileRepository fileRepository,
                IConfiguration configuration,
                IRequestContextManager requestContextManager
            ) : base(requestContextManager, translator)
        {
            _championshipFileRepository = championshipFileRepository;
            _championshipFileValidator = championshipFileValidator;
            _fileHelper = fileHelper;
            _notificationHelper = notificationHelper;
            _fileRepository = fileRepository;
            _authorizationManager = authorizationManager;
        }

        #endregion

        #region IChampionshipFileManager implementation

        public PaginatedResult<ChampionshipFile> Get(ChampionshipFileSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _championshipFileRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public ChampionshipFile Get(int id, ITransactionalContext? context = null)
        {
            //var contextUser = base.GetContextUser();
            //_authorizationManager.ValidatePermission(Domain.Enums.Action.ChampionshipFile_Get, id, contextUser.Id);

            var searchFilter = new ChampionshipFileSearchFilter() { Ids = new int[] { id } };

            var championshipFiles = _championshipFileRepository.Get(searchFilter: searchFilter, paginationFilter: null, sorting: null, context);

            var competitonFile = championshipFiles.Results.FirstOrDefault();
            if (competitonFile == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            return competitonFile;
        }

        public void Create(ChampionshipFile championshipFile, ITransactionalContext? context = null)
        {
            var contextUser = base.GetContextUser();

            _authorizationManager.ValidatePermission(contextUser.Id, Domain.Enums.Action.ChampionshipFile_Create, championshipFile.Championship.Id);

            if (context == null)
                context = _championshipFileRepository.GetTransactionalContext(TransactionContextScope.Internal);

            _championshipFileValidator.SetTransactionalContext(context);
            if (!_championshipFileValidator.IsValid(championshipFile, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _championshipFileValidator.Errors);

            try
            {
                championshipFile.File.Path = _fileHelper.SaveFile(Common.CommonValues.Directories.Files, championshipFile.Championship.Id.ToString(), championshipFile.File.Name, championshipFile.File.Content);

                _fileRepository.Create(championshipFile.File, context);

                _championshipFileRepository.Create(championshipFile, context);
                _championshipFileRepository.AssociateRaceClasses(championshipFile, context);

                context.Confirm();
            }
            catch (Exception)
            {
                if (context != null)
                    context.Cancel();

                if (!string.IsNullOrEmpty(championshipFile.File.Path))
                    _fileHelper.DeleteFile(Path.Combine(championshipFile.File.Path, championshipFile.File.Name));

                throw;
            }

            _notificationHelper.SendNotification(Notification.Enums.NotificationType.Championship_File_Upload, championshipFile);
        }

        public void Update(ChampionshipFile championshipFile, ITransactionalContext? context = null)
        {
            var contextUser = base.GetContextUser();

            _authorizationManager.ValidatePermission(contextUser.Id, Domain.Enums.Action.ChampionshipFile_Update, championshipFile.Championship.Id);

            if (context == null)
                context = _championshipFileRepository.GetTransactionalContext(TransactionContextScope.Internal);

            _championshipFileValidator.SetTransactionalContext(context);
            if (!_championshipFileValidator.IsValid(championshipFile, Scenario.Update))
                throw new FunctionalException(ErrorType.ValidationError, _championshipFileValidator.Errors);

            try
            {
                //championshipFile.File.Path = _fileHelper.SaveFile(Common.CommonValues.Directories.Files, championshipFile.Championship.Id.ToString(), championshipFile.File.Name, championshipFile.File.Content);

                //_fileRepository.Create(championshipFile.File, context);

                _championshipFileRepository.Update(championshipFile, context);
                _championshipFileRepository.AssociateRaceClasses(championshipFile, context);

                context.Confirm();
            }
            catch (Exception)
            {
                if (context != null)
                    context.Cancel();

                if (!string.IsNullOrEmpty(championshipFile.File.Path))
                    _fileHelper.DeleteFile(Path.Combine(championshipFile.File.Path, championshipFile.File.Name));

                throw;
            }

            _notificationHelper.SendNotification(Notification.Enums.NotificationType.Championship_File_Upload, championshipFile);
        }

        public void Delete(int id, ITransactionalContext? context = null)
        {
            var championshipFile = this.Get(id, context);

            var contextUser = base.GetContextUser();

            _authorizationManager.ValidatePermission(contextUser.Id, Domain.Enums.Action.ChampionshipFile_Delete, championshipFile.Championship.Id);

            //_championshipValidator.SetTransactionalContext(context);

            //if (!_championshipValidator.IsValid(championshipFile, Scenario.Delete))
            //    throw new FunctionalException(ErrorType.ValidationError, _championshipValidator.Errors);

            if (context == null)
                context = _championshipFileRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _championshipFileRepository.DeleteRaceClasses(championshipFile.Id, context);
                _championshipFileRepository.Delete(championshipFile.Id, context);

                _fileRepository.Delete(championshipFile.File.Id, context);

                _fileHelper.DeleteFile(Common.CommonValues.Directories.Files, championshipFile.Championship.Id.ToString(), championshipFile.File.Name);

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
