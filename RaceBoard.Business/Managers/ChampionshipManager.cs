using RaceBoard.Business.Helpers.Interfaces;
using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Business.Validators.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Common.Exceptions;
using RaceBoard.Common.Helpers;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;
using Enums = RaceBoard.Domain.Enums;

namespace RaceBoard.Business.Managers
{
    public class ChampionshipManager : AbstractManager, IChampionshipManager
    {
        private readonly IChampionshipRepository _championshipRepository;
        private readonly IChampionshipGroupRepository _championshipGroupRepository;
        private readonly IChampionshipMemberRepository _championshipMemberRepository; 
        private readonly IFileRepository _fileRepository;
        private readonly IUserAccessRepository _userAccessRepository;
        
        private readonly ICustomValidator<Championship> _championshipValidator;
        private readonly ICustomValidator<ChampionshipGroup> _championshipGroupValidator;

        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IFileHelper _fileHelper;
        private readonly INotificationHelper _notificationHelper;

        private readonly IAuthorizationManager _authorizationManager;

        #region Constructors

        public ChampionshipManager
            (
                IChampionshipRepository championshipRepository,
                IChampionshipGroupRepository championshipGrpupRepository,
                IChampionshipMemberRepository championshipMemberRepository,
                IFileRepository fileRepository,
                IUserAccessRepository userAccessRepository,
                ICustomValidator<Championship> championshipValidator,
                ICustomValidator<ChampionshipGroup> championshipGroupValidator,
                IChampionshipCommitteeBoatReturnRepository committeeBoatReturnRepository,
                ICustomValidator<ChampionshipCommitteeBoatReturn> committeeBoatReturnValidator,
                IDateTimeHelper dateTimeHelper,
                IRequestContextManager requestContextManager,
                IFileHelper fileHelper,
                INotificationHelper notificationHelper,
                ITranslator translator,
                IAuthorizationManager authorizationManager
            ) : base(requestContextManager, translator)
        {
            _championshipRepository = championshipRepository;
            _championshipMemberRepository = championshipMemberRepository;
            _championshipGroupRepository = championshipGrpupRepository;
            _fileRepository = fileRepository;
            _userAccessRepository = userAccessRepository;
            _championshipValidator = championshipValidator;
            _championshipGroupValidator = championshipGroupValidator;
            _dateTimeHelper = dateTimeHelper;
            _fileHelper = fileHelper;
            _notificationHelper = notificationHelper;
            _authorizationManager = authorizationManager;
        }

        #endregion

        #region IChampionshipManager implementation

        public PaginatedResult<Championship> Get(ChampionshipSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _championshipRepository.Get(searchFilter, paginationFilter, sorting, context);
        }

        public Championship Get(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new ChampionshipSearchFilter() { Ids = new int[] { id } };

            var championships = _championshipRepository.Get(searchFilter: searchFilter, paginationFilter: null, sorting: null, context);

            var championship = championships.Results.FirstOrDefault();
            if (championship == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            if (championship.ImageFile != null)
            {
                string basePath = _fileHelper.GetBasePath();

                championship.ImageFile.Path = basePath;
            }

            return championship;
        }

        public void Create(Championship championship, ITransactionalContext? context = null)
        {
            var contextUser = base.GetContextUser();
            _authorizationManager.ValidatePermission(contextUser.Id, Enums.Action.Championship_Create, championship.Organizations[0].Id);

            if (context == null)
                context = _championshipRepository.GetTransactionalContext(TransactionContextScope.Internal);

            var currentDate = _dateTimeHelper.GetCurrentTimestamp();

            _championshipValidator.SetTransactionalContext(context);

            if (!_championshipValidator.IsValid(championship, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _championshipValidator.Errors);

            try
            {
                _championshipRepository.Create(championship, context);
                _championshipRepository.SetOrganizations(championship.Id, championship.Organizations, context);

                var championshipMember = new ChampionshipMember()
                {
                    IsActive = true,
                    JoinDate = currentDate,
                    Championship = championship,
                    User = championship.CreationUser,
                    Role = new Role() { Id = (int)Enums.UserRole.Manager }
                };
                _championshipMemberRepository.Add(championshipMember, context);

                if (championship.ImageFile != null)
                {
                    string savedFilePath = _fileHelper.SaveFile(Common.CommonValues.Directories.Files, championship.Id.ToString(), championship.ImageFile.Name, championship.ImageFile.Content);
                    championship.ImageFile.Path = _fileHelper.RemoveBasePath(savedFilePath);

                    _fileRepository.Create(championship.ImageFile, context);
                    _championshipRepository.Update(championship, context);
                }

                context.Confirm();
            }
            catch (Exception)
            {
                if (context != null)
                    context.Cancel();

                if (!string.IsNullOrEmpty(championship.ImageFile?.Path))
                    _fileHelper.DeleteFile(Path.Combine(championship.ImageFile.Path, championship.ImageFile.Name));
                throw;
            }

            _notificationHelper.SendNotification(Notification.Enums.NotificationType.Championship_Creation, championship);
        }

        public void Update(Championship championship, ITransactionalContext? context = null)
        {
            var contextUser = base.GetContextUser();
            _authorizationManager.ValidatePermission(contextUser.Id, Enums.Action.Championship_Update, championship.Id);


            if (context == null)
                context = _championshipRepository.GetTransactionalContext(TransactionContextScope.Internal);

            _championshipValidator.SetTransactionalContext(context);

            if (!_championshipValidator.IsValid(championship, Scenario.Update))
                throw new FunctionalException(ErrorType.ValidationError, _championshipValidator.Errors);

            try
            {
                _championshipRepository.Update(championship, context);
                _championshipRepository.SetOrganizations(championship.Id, championship.Organizations, context);

                if (championship.ImageFile != null)
                {
                    championship.ImageFile.Path = _fileHelper.SaveFile(Common.CommonValues.Directories.Files, championship.Id.ToString(), championship.ImageFile.Name, championship.ImageFile.Content);
                    _fileRepository.Create(championship.ImageFile, context);
                    _championshipRepository.Update(championship, context);
                }

                _championshipRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _championshipRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void Delete(int id, ITransactionalContext? context = null)
        {
            var championship = this.Get(id, context);

            var contextUser = base.GetContextUser();

            _authorizationManager.ValidatePermission(contextUser.Id, Enums.Action.Championship_Delete, id);

            _championshipValidator.SetTransactionalContext(context);

            if (!_championshipValidator.IsValid(championship, Scenario.Delete))
                throw new FunctionalException(ErrorType.ValidationError, _championshipValidator.Errors);

            if (context == null)
                context = _championshipRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _championshipRepository.DeleteOrganizations(id, context);
                _championshipRepository.Delete(id, context);

                _championshipRepository.ConfirmTransactionalContext(context);

            }
            catch (Exception)
            {
                _championshipRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public List<ChampionshipGroup> GetGroups(int idChampionship, ITransactionalContext? context = null)
        {
            return _championshipGroupRepository.Get(idChampionship, context);
        }

        public ChampionshipGroup GetGroup(int id, ITransactionalContext? context = null)
        {
            return _championshipGroupRepository.GetById(id, context);
        }

        public void CreateGroup(ChampionshipGroup championshipGroup, ITransactionalContext? context = null)
        {
            if (context == null)
                context = _championshipGroupRepository.GetTransactionalContext(TransactionContextScope.Internal);

            _championshipGroupValidator.SetTransactionalContext(context);

            if (!_championshipGroupValidator.IsValid(championshipGroup, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _championshipGroupValidator.Errors);

            try
            {
                _championshipGroupRepository.Create(championshipGroup, context);
                _championshipGroupRepository.DeleteRaceClasses(championshipGroup.Id, context);
                _championshipGroupRepository.CreateRaceClasses(championshipGroup.Id, championshipGroup.RaceClasses, context);

                _championshipGroupRepository.ConfirmTransactionalContext(context);
            }
            catch (Exception)
            {
                _championshipGroupRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void UpdateGroup(ChampionshipGroup championshipGroup, ITransactionalContext? context = null)
        {
            if (context == null)
                context = _championshipGroupRepository.GetTransactionalContext(TransactionContextScope.Internal);

            _championshipGroupValidator.SetTransactionalContext(context);

            if (!_championshipGroupValidator.IsValid(championshipGroup, Scenario.Update))
                throw new FunctionalException(ErrorType.ValidationError, _championshipGroupValidator.Errors);

            try
            {
                _championshipGroupRepository.Update(championshipGroup, context);
                _championshipGroupRepository.DeleteRaceClasses(championshipGroup.Id, context);
                _championshipGroupRepository.CreateRaceClasses(championshipGroup.Id, championshipGroup.RaceClasses, context);

                _championshipGroupRepository.ConfirmTransactionalContext(context);
            }
            catch (Exception)
            {
                _championshipGroupRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        public void DeleteGroup(int idChampionshipGroup, ITransactionalContext? context = null)
        {
            var championshipGroup = this.GetGroup(idChampionshipGroup, context);

            _championshipGroupValidator.SetTransactionalContext(context);

            if (!_championshipGroupValidator.IsValid(championshipGroup, Scenario.Delete))
                throw new FunctionalException(ErrorType.ValidationError, _championshipGroupValidator.Errors);

            if (context == null)
                context = _championshipRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _championshipGroupRepository.DeleteRaceClasses(idChampionshipGroup, context);
                _championshipGroupRepository.Delete(idChampionshipGroup, context);

                _championshipRepository.ConfirmTransactionalContext(context);
            }
            catch (Exception)
            {
                _championshipRepository.CancelTransactionalContext(context);
                throw;
            }
        }

        #endregion
    }
}