using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
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
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;

namespace RaceBoard.Business.Managers
{
    public class RequestManager : AbstractManager, IRequestManager
    {
        private readonly IAuthorizationManager _authorizationManager;

        private readonly IEquipmentChangeRequestRepository _equipmentChangeRequestRepository;
        private readonly ICrewChangeRequestRepository _crewChangeRequestRepository;
        private readonly IHearingRequestRepository _hearingRequestRepository;
        private readonly IHearingRequestTypeRepository _hearingRequestTypeRepository;
        private readonly IFileRepository _fileRepository;

        private readonly IUserSettingsManager _userSettingsManager;

        private readonly IFileHelper _fileHelper;
        private readonly IDateTimeHelper _dateTimeHelper;

        private readonly ICustomValidator<EquipmentChangeRequest> _equipmentChangeRequestValidator;
        private readonly ICustomValidator<CrewChangeRequest> _crewChangeRequestValidator;
        private readonly ICustomValidator<HearingRequest> _hearingRequestValidator;


        private Color _COLOR_LIGHT_GREY = Colors.Grey.Lighten3;
        private Color _COLOR_GREY = Colors.Grey.Darken3;
        private Color _COLOR_WHITE = Colors.White;
        private Color _COLOR_DATA = Colors.Purple.Medium;
        private bool _isEmpty = true;
        private HearingRequest _request;
        private UserSettings _userSettings;

        #region Constructors

        public RequestManager
            (
                IAuthorizationManager authorizationManager,
                IEquipmentChangeRequestRepository equipmentChangeRequestRepository,
                ICrewChangeRequestRepository crewChangeRequestRepository,
                IHearingRequestRepository hearingRequestRepository,
                IHearingRequestTypeRepository hearingRequestTypeRepository,
                IFileRepository fileRepository,
                IUserSettingsManager userSettingsManager,
                IFileHelper fileHelper,
                IDateTimeHelper dateTimeHelper,
                ICustomValidator<EquipmentChangeRequest> equipmentChangeRequestValidator,
                ICustomValidator<CrewChangeRequest> crewChangeRequestValidator,
                ICustomValidator<HearingRequest> hearingRequestValidator,
                IRequestContextManager requestContextManager,
                ITranslator translator
            ) : base(requestContextManager, translator)
        {
            _authorizationManager = authorizationManager;

            _crewChangeRequestRepository = crewChangeRequestRepository;
            _equipmentChangeRequestRepository = equipmentChangeRequestRepository;
            _hearingRequestRepository = hearingRequestRepository;
            _hearingRequestTypeRepository = hearingRequestTypeRepository;
            _fileRepository = fileRepository;

            _userSettingsManager = userSettingsManager;

            _crewChangeRequestValidator = crewChangeRequestValidator;
            _equipmentChangeRequestValidator = equipmentChangeRequestValidator;
            _hearingRequestValidator = hearingRequestValidator;

            _fileHelper = fileHelper;
            _dateTimeHelper = dateTimeHelper;
        }

        #endregion

        #region IChangeRequestManager implementation

        #region Crew Change Request

        public void CreateCrewChangeRequest(CrewChangeRequest crewChangeRequest, ITransactionalContext? context = null)
        {
            var contextUser = base.GetContextUser();

            _authorizationManager.ValidatePermission(Enums.Action.TeamCrewChangeRequest_Create, crewChangeRequest.Team.Id, contextUser.Id);

            crewChangeRequest.Status = new ChangeRequestStatus() { Id = (int)Enums.RequestStatus.Submitted };
            crewChangeRequest.RequestUser = contextUser;
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

        public void UpdateCrewChangeRequest(CrewChangeRequest crewChangeRequest, ITransactionalContext? context = null)
        {
            throw new NotImplementedException();
        }

        public CrewChangeRequest GetCrewChangeRequest(int id, ITransactionalContext? context = null)
        {
            var searchFilter = new ChangeRequestSearchFilter() { Ids = new[] { id } };

            var changeRequests = _crewChangeRequestRepository.Get(searchFilter, paginationFilter: null, sorting: null, context: context);

            var changeRequest = changeRequests.Results.FirstOrDefault();
            if (changeRequest == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            return changeRequest;
        }

        public PaginatedResult<CrewChangeRequest> GetCrewChangeRequests(ChangeRequestSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _crewChangeRequestRepository.Get(searchFilter, paginationFilter, sorting);
        }

        #endregion

        #region Equipment Change Request

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

        public void CreateEquipmentChangeRequest(EquipmentChangeRequest equipmentChangeRequest, ITransactionalContext? context = null)
        {
            var contextUser = base.GetContextUser();
            _authorizationManager.ValidatePermission(Enums.Action.TeamEquipmentChangeRequest_Create, equipmentChangeRequest.Team.Id, contextUser.Id);

            equipmentChangeRequest.Status = new ChangeRequestStatus() { Id = (int)Enums.RequestStatus.Submitted };
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

        public void UpdateEquipmentChangeRequest(CrewChangeRequest crewChangeRequest, ITransactionalContext? context = null)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Hearing Request

        public PaginatedResult<HearingRequestType> GetHearingRequestTypes(PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _hearingRequestTypeRepository.Get(paginationFilter, sorting);
        }

        public PaginatedResult<HearingRequest> GetHearingRequests(HearingRequestSearchFilter? searchFilter = null, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _hearingRequestRepository.Get(searchFilter, paginationFilter, sorting);
        }

        public HearingRequest GetHearingRequest(int id, ITransactionalContext? context = null)
        {
            var hearing = _hearingRequestRepository.Get(id, context);
            if (hearing == null)
                throw new FunctionalException(ErrorType.NotFound, this.Translate("RecordNotFound"));

            var protestor = _hearingRequestRepository.GetProtestor(id, context: context);
            hearing.Protestor = protestor;

            var protestees = _hearingRequestRepository.GetProtestees(id, context: context);
            hearing.Protestees = protestees;

            var incident = _hearingRequestRepository.GetIncident(id, context: context);
            hearing.Incident = incident;

            return hearing;
        }

        public void CreateHearingRequest(HearingRequest hearingRequest, ITransactionalContext? context = null)
        {
            var contextUser = base.GetContextUser();

            _authorizationManager.ValidatePermission(Enums.Action.TeamHearingRequest_Create, hearingRequest.Team.Id, contextUser.Id);

            hearingRequest.Status = new HearingRequestStatus() { Id = (int)Enums.RequestStatus.Submitted };
            hearingRequest.RequestUser = contextUser;
            hearingRequest.CreationDate = _dateTimeHelper.GetCurrentTimestamp();

            _hearingRequestValidator.SetTransactionalContext(context);

            if (!_hearingRequestValidator.IsValid(hearingRequest, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _hearingRequestValidator.Errors);

            if (context == null)
                context = _hearingRequestRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _hearingRequestRepository.Create(hearingRequest, context);
                _hearingRequestRepository.CreateProtestor(hearingRequest, context);
                _hearingRequestRepository.CreateProtestorNotice(hearingRequest, context);
                _hearingRequestRepository.CreateProtestees(hearingRequest, context);
                _hearingRequestRepository.CreateIncident(hearingRequest, context);

                context.Confirm();
            }
            catch (Exception)
            {
                if (context != null)
                    context.Cancel();

                throw;
            }
        }

        public void UpdateHearingRequest(HearingRequest hearingRequest, ITransactionalContext? context = null)
        {
            var contextUser = base.GetContextUser();

            _authorizationManager.ValidatePermission(Enums.Action.TeamHearingRequest_Create, hearingRequest.Team.Id, contextUser.Id);

            hearingRequest.Status = new HearingRequestStatus() { Id = (int)Enums.RequestStatus.Submitted };
            hearingRequest.RequestUser = contextUser;
            hearingRequest.CreationDate = _dateTimeHelper.GetCurrentTimestamp();

            _hearingRequestValidator.SetTransactionalContext(context);

            if (!_hearingRequestValidator.IsValid(hearingRequest, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _hearingRequestValidator.Errors);

            if (context == null)
                context = _hearingRequestRepository.GetTransactionalContext(TransactionContextScope.Internal);

            try
            {
                _hearingRequestRepository.Create(hearingRequest, context);
                _hearingRequestRepository.CreateProtestor(hearingRequest, context);
                _hearingRequestRepository.CreateProtestorNotice(hearingRequest, context);
                _hearingRequestRepository.CreateProtestees(hearingRequest, context);
                _hearingRequestRepository.CreateIncident(hearingRequest, context);

                context.Confirm();
            }
            catch (Exception)
            {
                if (context != null)
                    context.Cancel();

                throw;
            }
        }

        public byte[] RenderHearingRequest(HearingRequest hearingRequest, ITransactionalContext? context = null)
        {
            var contextUser = base.GetContextUser();

            _isEmpty = hearingRequest == null;
            _request = hearingRequest != null ? hearingRequest : BuildEmptyHearingRequestObject();

            _userSettings = _userSettingsManager.Get(contextUser.Id);

            QuestPDF.Settings.License = LicenseType.Community;

            var defaultTextStyle = TextStyle.Default
                .FontFamily("Arial")
                .FontSize(8);

            var doc = QuestPDF.Fluent.Document.Create
                (
                    container =>
                    {
                        container.Page(page =>
                        {
                            page.DefaultTextStyle(defaultTextStyle);

                            page.Size(PageSizes.A4);
                            page.Margin(1f, Unit.Centimetre);

                            page.Header().Element(AddHeader);
                            page.Content().Element(AddContent);
                            page.Footer().Element(AddFooter);
                        });
                    }
                );

            return doc.GeneratePdf();
        }

        #endregion

        #endregion

        private void AddHeader(IContainer container)
        {
            var defaultTextStyle = TextStyle.Default
            .FontFamily("Arial")
            .FontSize(7);

            IContainer CellStyleTopHeader(IContainer container) => container.DefaultTextStyle(defaultTextStyle);

            var defaultTextStyleWithValue = TextStyle.Default
                .FontFamily("Arial")
                .FontSize(7)
                .FontColor(_COLOR_DATA);

            IContainer CellStyleWithValue(IContainer container) => container.DefaultTextStyle(defaultTextStyleWithValue);


            container.Column(column =>
            {
                column.Spacing(5);

                column.Item().Row(row =>
                {
                    row.ConstantItem(4.0f, Unit.Centimetre).Column(column =>
                    {
                        column.Item().Element(CellStyleTopHeader).Text(Translate("ReceivedByRaceOfficer"));
                    });

                    row.ConstantItem(1.3f, Unit.Centimetre).Column(column =>
                    {
                        column.Item().Element(CellStyleTopHeader).Text($"{Translate("Hearing#")}");
                    });
                    row.ConstantItem(0.5f, Unit.Centimetre).Column(column =>
                    {
                        var element = column.Item();

                        if (_isEmpty)
                        {
                            column.Item().Element(CellStyleTopHeader).Text(". . . .");
                        }
                        else
                        {
                            column.Item().Element(CellStyleWithValue).Text(_request.RequestNumber.ToString()).Bold();
                        }
                    });
                    //
                    row.ConstantItem(0.8f, Unit.Centimetre).Column(column =>
                    {
                        column.Item().Element(CellStyleTopHeader).Text(". . . . ");
                    });

                    row.ConstantItem(1.6f, Unit.Centimetre).Column(column =>
                    {
                        column.Item().Element(CellStyleTopHeader).Text(Translate("DateAndTime"));
                    });
                    row.ConstantItem(1.25f, Unit.Centimetre).Column(column =>
                    {
                        var element = column.Item();

                        if (_isEmpty)
                        {
                            element.Element(CellStyleTopHeader).Text(". . . . . . . . .");
                        }
                        else
                        {
                            var currentTimestamp = _dateTimeHelper.GetCurrentTimestamp();
                            string currentDateAndTime = _dateTimeHelper.GetFormattedTimestamp(currentTimestamp, _userSettings.TimeZone.Identifier, _userSettings.DateFormat.Format);

                            element.Element(CellStyleWithValue).Text(currentDateAndTime).Bold();
                        }
                    });
                    row.ConstantItem(2.1f, Unit.Centimetre).Column(column =>
                    {
                        column.Item().Element(CellStyleTopHeader).Text(". . . . . . . . . . . .");
                    });

                    row.ConstantItem(5.9f, Unit.Centimetre).Column(column =>
                    {
                        column.Item().Element(CellStyleTopHeader).Text($"{Translate("Signature")} . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . ");
                    });
                });

                column.Item().Row(row =>
                {
                    row.RelativeItem().Column(column =>
                    {
                        column.Item()
                            .PaddingVertical(5)
                            .LineHorizontal(2)
                            .LineColor(_COLOR_GREY);
                    });
                });

                column.Item().Row(row =>
                {
                    row.RelativeItem(30).Column(column =>
                    {
                        column.Item().Text(Translate("HearingForm").ToUpper()).FontSize(14).Bold();
                    });

                    row.RelativeItem(50).Column(column =>
                    {
                        column.Item().Text($" - {Translate("AlsoForRepairAndReopenings").ToLower()}").Bold().LineHeight(2.5f);
                    });
                });
            });
        }

        private void AddFooter(IContainer container)
        {
            container.AlignCenter().Text(x =>
            {
                x.CurrentPageNumber();
                x.Span(" / ");
                x.TotalPages();
            });

        }

        private void AddContent(IContainer container)
        {
            string? requestCreationDate = _request.CreationDate == default(DateTimeOffset) ? null : _request.CreationDate.ToString(_userSettings.DateFormat.Format);
            string? requestIncidentTime = _request.Incident.Time == default(TimeSpan) ? null : $"{_request.Incident.Time.Hours}:{_request.Incident.Time.Minutes} - ";

            IContainer CellStyleWithNoBorderAndTopPadding(IContainer container) => container.Border(0).PaddingTop(3).PaddingBottom(1).PaddingLeft(1).PaddingRight(1);
            //IContainer CellStyleWithNoBorder(IContainer container) => container.Border(0).Padding(1);

            var defaultTextStyle = TextStyle.Default
                .FontColor(_COLOR_DATA);

            IContainer CellStyle(IContainer container) => container.Border(0.5f).Padding(1).DefaultTextStyle(defaultTextStyle);

            float spacingBetweenSections = 10f;

            container.Column(column =>
            {
                // 1. Event
                column.Spacing(spacingBetweenSections);
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(7.4f, Unit.Centimetre);
                        columns.ConstantColumn(7.5f, Unit.Centimetre);
                        columns.ConstantColumn(2.0f, Unit.Centimetre);
                        columns.ConstantColumn(2.0f, Unit.Centimetre);
                    });

                    table.Cell().ColumnSpan(4)
                        .Background(_COLOR_GREY).Padding(2)
                        .Text("1. EVENTO").FontColor(_COLOR_WHITE).Bold();

                    table.Cell().Element(CellStyleWithNoBorderAndTopPadding).Text("EVENTO").Bold();
                    table.Cell().Element(CellStyleWithNoBorderAndTopPadding).Text("Autoridad organizadora").Bold();
                    table.Cell().Element(CellStyleWithNoBorderAndTopPadding).Text("Fecha").Bold();
                    table.Cell().Element(CellStyleWithNoBorderAndTopPadding).Text("Regata N°").Bold();

                    table.Cell().Element(CellStyle).Text(ReturnEmptyStringIfNull(_request.Team.Championship.Name));
                    table.Cell().Element(CellStyle).Text(ReturnEmptyStringIfNull(_request.Team.Organization.Name));
                    table.Cell().Element(CellStyle).Text(ReturnEmptyStringIfNull(requestCreationDate));
                    table.Cell().Element(CellStyle).Text(ReturnEmptyStringIfNull(_request.RaceNumber));
                });

                // 2. Audience Type
                column.Spacing(spacingBetweenSections);
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(8.4f, Unit.Centimetre);
                        columns.ConstantColumn(1.0f, Unit.Centimetre);
                        columns.ConstantColumn(8.5f, Unit.Centimetre);
                        columns.ConstantColumn(1.0f, Unit.Centimetre);
                    });

                    table.Cell().ColumnSpan(4).Background(_COLOR_GREY).Padding(2).Text("2. TIPO DE AUDIENCIA").FontColor(_COLOR_WHITE).Bold();

                    table.Cell().Element(CellStyleWithNoBorderAndTopPadding).Text("Protesta de un barco a otro").Bold();
                    table.Cell().AlignCenter().Text(GetCheckBoxSymbol(false)).FontSize(12);
                    table.Cell().Element(CellStyleWithNoBorderAndTopPadding).Text("Pedido de reparación por barco o comisión de regata").Bold();
                    table.Cell().AlignCenter().Text(GetCheckBoxSymbol(false)).FontSize(12);

                    table.Cell().Element(CellStyleWithNoBorderAndTopPadding).Text("Protesta de comisión de regata a un barco").Bold();
                    table.Cell().AlignCenter().Text(GetCheckBoxSymbol(false)).FontSize(12);
                    table.Cell().Element(CellStyleWithNoBorderAndTopPadding).Text("Consideración de reparación por com. de protestas").Bold();
                    table.Cell().AlignCenter().Text(GetCheckBoxSymbol(false)).FontSize(12);

                    table.Cell().Element(CellStyleWithNoBorderAndTopPadding).Text("Protesta de comisión de protestas a un barco").Bold();
                    table.Cell().AlignCenter().Text(GetCheckBoxSymbol(false)).FontSize(12);
                    table.Cell().Element(CellStyleWithNoBorderAndTopPadding).Text("Pedido de reapertura por barco o comisión de regata").Bold();
                    table.Cell().AlignCenter().Text(GetCheckBoxSymbol(false)).FontSize(12);

                    table.Cell().Element(CellStyleWithNoBorderAndTopPadding).Text("").Bold();
                    table.Cell().AlignCenter().Text("").FontSize(12);
                    table.Cell().Element(CellStyleWithNoBorderAndTopPadding).Text("Consideración de reapertura por com. de protestas").Bold();
                    table.Cell().AlignCenter().Text(GetCheckBoxSymbol(false)).FontSize(12);

                });

                // 3. Protestor
                column.Spacing(spacingBetweenSections);
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(7.4f, Unit.Centimetre);
                        columns.ConstantColumn(8.0f, Unit.Centimetre);
                        columns.ConstantColumn(3.5f, Unit.Centimetre);
                    });

                    table.Cell().ColumnSpan(3)
                        .Background(_COLOR_GREY).Padding(2)
                        .Text("3. BARCO QUE PROTESTA, PIDE REPARACIÓN O REAPERTURA").FontColor(_COLOR_WHITE).Bold();

                    table.Cell().Element(CellStyleWithNoBorderAndTopPadding).Text("Clase").Bold();
                    table.Cell().Element(CellStyleWithNoBorderAndTopPadding).Text("Nombre del barco").Bold();
                    table.Cell().Element(CellStyleWithNoBorderAndTopPadding).Text("Vela N°").Bold();
                    table.Cell().Element(CellStyle).Text(ReturnEmptyStringIfNull(_request.Team.RaceClass.Name));
                    table.Cell().Element(CellStyle).Text(ReturnEmptyStringIfNull(_request.Protestor.Boat.Name));
                    table.Cell().Element(CellStyle).Text(ReturnEmptyStringIfNull(_request.Protestor.Boat.SailNumber));

                    table.Cell().Element(CellStyleWithNoBorderAndTopPadding).Text("Representado por").Bold();
                    table.Cell().Element(CellStyleWithNoBorderAndTopPadding).Text("Domicilio").Bold();
                    table.Cell().Element(CellStyleWithNoBorderAndTopPadding).Text("Tel.").Bold();
                    table.Cell().Element(CellStyle).Text(ReturnEmptyStringIfNull(_request.RequestPerson.Fullname));
                    table.Cell().Element(CellStyle).Text(ReturnEmptyStringIfNull(_request.Protestor.Address));
                    table.Cell().Element(CellStyle).Text(ReturnEmptyStringIfNull(_request.Protestor.PhoneNumber));
                });

                // 4. Protestee
                column.Spacing(spacingBetweenSections);
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(7.4f, Unit.Centimetre);
                        columns.ConstantColumn(8.0f, Unit.Centimetre);
                        columns.ConstantColumn(3.5f, Unit.Centimetre);
                    });

                    table.Cell().ColumnSpan(3)
                        .Background(_COLOR_GREY).Padding(2)
                        .Text("4. BARCO(S) PROTESTADO(S) O CONSIDERADO(S) PARA UNA REPARACIÓN").FontColor(_COLOR_WHITE).Bold();

                    for (int i = 0; i < _request.Protestees.Protestees.Count; i++)
                    {
                        var protestee = _request.Protestees.Protestees[i];

                        table.Cell().Element(CellStyleWithNoBorderAndTopPadding).Text("Clase").Bold();
                        table.Cell().Element(CellStyleWithNoBorderAndTopPadding).Text("Nombre del barco").Bold();
                        table.Cell().Element(CellStyleWithNoBorderAndTopPadding).Text("Vela N°").Bold();
                        table.Cell().Element(CellStyle).Text(ReturnEmptyStringIfNull(protestee.TeamBoat.Boat.RaceClass.Name));
                        table.Cell().Element(CellStyle).Text(ReturnEmptyStringIfNull(protestee.TeamBoat.Boat.Name));
                        table.Cell().Element(CellStyle).Text(ReturnEmptyStringIfNull(protestee.TeamBoat.Boat.SailNumber));
                    }
                });

                // 5. Incident
                column.Spacing(spacingBetweenSections);
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(7.5f, Unit.Centimetre);
                        columns.ConstantColumn(11.4f, Unit.Centimetre);
                    });

                    table.Cell().ColumnSpan(2).Background(_COLOR_GREY).Padding(2).Text("5. INCIDENTE").FontColor(_COLOR_WHITE).Bold();

                    table.Cell().ColumnSpan(2).Element(CellStyleWithNoBorderAndTopPadding).Text("Hora y lugar del incidente").Bold();
                    table.Cell().ColumnSpan(2).Element(CellStyle).Text(ReturnEmptyStringIfNull(requestIncidentTime) + _request.Incident.Place);

                    table.Cell().Element(CellStyleWithNoBorderAndTopPadding).Text("Reglas que se habrían infringido").Bold();
                    table.Cell().Element(CellStyleWithNoBorderAndTopPadding).Text("Testigos").Bold();
                    table.Cell().Element(CellStyle).Text(ReturnEmptyStringIfNull(_request.Incident.BrokenRules));
                    table.Cell().Element(CellStyle).Text(ReturnEmptyStringIfNull(_request.Incident.Witnesses));
                });

                // 6. Protestor Notice
                column.Spacing(spacingBetweenSections);
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(5.0f, Unit.Centimetre);
                        columns.ConstantColumn(1.0f, Unit.Centimetre);
                        columns.ConstantColumn(7.5f, Unit.Centimetre);
                        columns.ConstantColumn(5.4f, Unit.Centimetre);
                    });

                    table.Cell().ColumnSpan(4).Background(_COLOR_GREY).Padding(2).Text("6. AVISO AL PROTESTADO ¿Cómo comunicó Ud. al protestado su intención de protestar?").FontColor(_COLOR_WHITE).Bold();

                    table.Cell().RowSpan(2).Element(CellStyleWithNoBorderAndTopPadding).Text("En voz alta").Bold();
                    table.Cell().RowSpan(2).AlignCenter().Text(GetCheckBoxSymbol(_request.Protestor.Notice.Hailing)).FontSize(12);
                    table.Cell().Element(CellStyleWithNoBorderAndTopPadding).Text("Cuándo?").Bold();
                    table.Cell().Element(CellStyleWithNoBorderAndTopPadding).Text("Palabra(s) usada(s)").Bold();
                    table.Cell().Element(CellStyle).Text(ReturnEmptyStringIfNull(_request.Protestor.Notice.HailingWhen));
                    table.Cell().Element(CellStyle).Text(ReturnEmptyStringIfNull(_request.Protestor.Notice.HailingWordsUsed));

                    table.Cell().RowSpan(2).Element(CellStyleWithNoBorderAndTopPadding).Text("Desplegando una bandera roja").Bold();
                    table.Cell().RowSpan(2).AlignCenter().Text(GetCheckBoxSymbol(_request.Protestor.Notice.RedFlag)).FontSize(12);
                    table.Cell().ColumnSpan(2).Element(CellStyleWithNoBorderAndTopPadding).Text("Cuándo?").Bold();
                    table.Cell().ColumnSpan(2).Element(CellStyle).Text(ReturnEmptyStringIfNull(_request.Protestor.Notice.RedFlagWhen));

                    table.Cell().RowSpan(2).Element(CellStyleWithNoBorderAndTopPadding).Text("Informándole de otro modo").Bold();
                    table.Cell().RowSpan(2).AlignCenter().Text(GetCheckBoxSymbol(_request.Protestor.Notice.Other)).FontSize(12);
                    table.Cell().ColumnSpan(2).Element(CellStyleWithNoBorderAndTopPadding).Text("Aclare cómo").Bold();
                    table.Cell().ColumnSpan(2).Element(CellStyle).Text(ReturnEmptyStringIfNull(_request.Protestor.Notice.OtherHow));
                });

                // 7. Incident description
                //column.Spacing(spacingBetweenSections);
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(18.9f, Unit.Centimetre);
                    });

                    table.Cell().Background(_COLOR_GREY).Padding(2).Text("7. DESCRIPCIÓN DEL INCIDENTE").FontColor(_COLOR_WHITE).Bold();

                    if (!string.IsNullOrEmpty(_request.Incident.Details))
                        table.Cell().Height(200).Background(_COLOR_LIGHT_GREY).Text(ReturnEmptyStringIfNull(_request.Incident.Details));
                    else
                        table.Cell().Height(200).Background(_COLOR_LIGHT_GREY).Text(".").FontColor(_COLOR_LIGHT_GREY); //.Placeholder(".").;
                });
            });

        }

        private string GetCheckBoxSymbol(bool isChecked)
        {
            return isChecked ? "☑" : "☐";
        }

        private string ReturnEmptyStringIfNull(string? value)
        {
            return value ?? "";
        }

        private HearingRequest BuildEmptyHearingRequestObject()
        {
            return new HearingRequest()
            {
                CreationDate = default(DateTimeOffset),
                RequestPerson = new Person() { Firstname = "", Lastname = "" },
                RaceNumber = "",
                Team = new Team()
                {
                    Championship = new Championship() { Name = "" },
                    Organization = new Organization() { Name = "" },
                    RaceClass = new RaceClass() { Name = "" }
                },
                Protestor = new HearingRequestProtestor()
                {
                    Boat = new Boat() { Name = "" },
                    Notice = new HearingRequestProtestorNotice() { HailingWhen = "", HailingWordsUsed = "", RedFlagWhen = "", OtherHow = "", OtherWhere = "" }
                },
                Protestees = new HearingRequestProtestees()
                {
                    Protestees = new List<HearingRequestProtestee>()
                        {
                            new HearingRequestProtestee() { TeamBoat = new TeamBoat() { Boat = new Boat() { Name = "", SailNumber = "", RaceClass = new RaceClass() { Name = "" } } } },
                            new HearingRequestProtestee() { TeamBoat = new TeamBoat() { Boat = new Boat() { Name = "", SailNumber = "", RaceClass = new RaceClass() { Name = "" } } } },
                            new HearingRequestProtestee() { TeamBoat = new TeamBoat() { Boat = new Boat() { Name = "", SailNumber = "", RaceClass = new RaceClass() { Name = "" } } } }
                        }
                },
                Incident = new HearingRequestIncident() { Place = "", Witnesses = "", BrokenRules = "", Details = "" }
            };
        }
    }
}
