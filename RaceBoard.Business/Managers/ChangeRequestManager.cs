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
using Enums = RaceBoard.Domain.Enums;

namespace RaceBoard.Business.Managers
{
    public class ChangeRequestManager : AbstractManager, IChangeRequestManager
    {
        private readonly IAuthorizationManager _authorizationManager;

        private readonly IEquipmentChangeRequestRepository _equipmentChangeRequestRepository;
        private readonly ICrewChangeRequestRepository _crewChangeRequestRepository;
        private readonly IFileRepository _fileRepository;

        private readonly IFileHelper _fileHelper;
        private readonly IDateTimeHelper _dateTimeHelper;

        private readonly ICustomValidator<EquipmentChangeRequest> _equipmentChangeRequestValidator;
        private readonly ICustomValidator<CrewChangeRequest> _crewChangeRequestValidator;

        #region Constructors

        public ChangeRequestManager
            (
                IAuthorizationManager authorizationManager,
                IEquipmentChangeRequestRepository equipmentChangeRequestRepository,
                ICrewChangeRequestRepository crewChangeRequestRepository,
                IFileRepository fileRepository,
                IFileHelper fileHelper,
                IDateTimeHelper dateTimeHelper,
                ICustomValidator<EquipmentChangeRequest> equipmentChangeRequestValidator,
                ICustomValidator<CrewChangeRequest> crewChangeRequestValidator,
                IRequestContextManager requestContextManager,
                ITranslator translator
            ) : base(requestContextManager, translator)
        {
            _authorizationManager = authorizationManager;

            _equipmentChangeRequestRepository = equipmentChangeRequestRepository;
            _equipmentChangeRequestValidator = equipmentChangeRequestValidator;
            _fileRepository = fileRepository;

            _fileHelper = fileHelper;
            _dateTimeHelper = dateTimeHelper;

            _crewChangeRequestRepository = crewChangeRequestRepository;
            _crewChangeRequestValidator = crewChangeRequestValidator;
        }

        #endregion

        #region IChangeRequestManager implementation

        public void CreateCrewChangeRequest(CrewChangeRequest crewChangeRequest, ITransactionalContext? context = null)
        {
            var contextUser = base.GetContextUser();
            _authorizationManager.ValidatePermission(Enums.Action.TeamCrewChangeRequest_Create, crewChangeRequest.Team.Id, contextUser.Id);

            crewChangeRequest.Status = new RequestStatus() { Id = (int)Enums.RequestStatus.Submitted };
            crewChangeRequest.RequestUser = base.GetContextUser();
            crewChangeRequest.CreationDate = _dateTimeHelper.GetCurrentTimestamp();

            _crewChangeRequestValidator.SetTransactionalContext(context);

            if (!_crewChangeRequestValidator.IsValid(crewChangeRequest, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _crewChangeRequestValidator.Errors);

            if (context == null)
                context = _crewChangeRequestRepository.GetTransactionalContext(TransactionContextScope.Internal);

            Domain.File? file = null;

            try
            {
                if (crewChangeRequest.File != null)
                {
                    file = crewChangeRequest.File;
                    file.Path = _fileHelper.SaveFile(Common.CommonValues.Directories.Files, crewChangeRequest.Id.ToString(), file.Name, file.Content);
                    _fileRepository.Create(file, context);
                }

                _crewChangeRequestRepository.Create(crewChangeRequest, context);

                context.Confirm();
            }
            catch (Exception)
            {
                if (context != null)
                    context.Cancel();

                if (file != null)
                    _fileHelper.DeleteFile(Path.Combine(file.Path, file.Name));

                throw;
            }
        }

        public void CreateEquipmentChangeRequest(EquipmentChangeRequest equipmentChangeRequest, ITransactionalContext? context = null)
        {
            var contextUser = base.GetContextUser();
            _authorizationManager.ValidatePermission(Enums.Action.TeamEquipmentChangeRequest_Create, equipmentChangeRequest.Team.Id, contextUser.Id);

            equipmentChangeRequest.Status = new RequestStatus() { Id = (int)Enums.RequestStatus.Submitted };
            equipmentChangeRequest.RequestUser = contextUser;
            equipmentChangeRequest.CreationDate = _dateTimeHelper.GetCurrentTimestamp();

            _equipmentChangeRequestValidator.SetTransactionalContext(context);

            if (!_equipmentChangeRequestValidator.IsValid(equipmentChangeRequest, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _equipmentChangeRequestValidator.Errors);

            if (context == null)
                context = _equipmentChangeRequestRepository.GetTransactionalContext(TransactionContextScope.Internal);

            Domain.File? file = null;

            try
            {
                if (equipmentChangeRequest.File != null)
                {
                    file = equipmentChangeRequest.File;
                    file.Path = _fileHelper.SaveFile(Common.CommonValues.Directories.Files, equipmentChangeRequest.Id.ToString(), file.Name, file.Content);
                    _fileRepository.Create(file, context);
                }

                _equipmentChangeRequestRepository.Create(equipmentChangeRequest, context);

                context.Confirm();
            }
            catch (Exception)
            {
                if (context != null)
                    context.Cancel();

                if (file != null)
                    _fileHelper.DeleteFile(Path.Combine(file.Path, file.Name));

                throw;
            }
        }

        public CrewChangeRequest GetCrewChangeRequest(int id, ITransactionalContext? context = null)
        {
            throw new NotImplementedException();
        }

        public PaginatedResult<CrewChangeRequest> GetCrewChangeRequests(ChangeRequestSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _crewChangeRequestRepository.Get(searchFilter, paginationFilter, sorting);
        }

        public EquipmentChangeRequest GetEquipmentChangeRequest(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new ChangeRequestSearchFilter() { Ids = new[] { id } };

            var changeRequests = _equipmentChangeRequestRepository.Get(searchFilter, paginationFilter: null, sorting: null, context: context);

            var changeRequest = changeRequests.Results.FirstOrDefault();
            if (changeRequest == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            return changeRequest;
        }

        public PaginatedResult<EquipmentChangeRequest> GetEquipmentChangeRequests(ChangeRequestSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _equipmentChangeRequestRepository.Get(searchFilter, paginationFilter, sorting);
        }

        public void UpdateCrewChangeRequest(CrewChangeRequest crewChangeRequest, ITransactionalContext? context = null)
        {
            throw new NotImplementedException();
        }

        public void UpdateEquipmentChangeRequest(CrewChangeRequest crewChangeRequest, ITransactionalContext? context = null)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
