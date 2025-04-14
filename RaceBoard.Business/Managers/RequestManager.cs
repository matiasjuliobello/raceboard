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
using System.Reflection.Metadata;
using Enums = RaceBoard.Domain.Enums;

using System.Diagnostics;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;
using System.Data.Common;
using System.IO;


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

        private readonly IFileHelper _fileHelper;
        private readonly IDateTimeHelper _dateTimeHelper;

        private readonly ICustomValidator<EquipmentChangeRequest> _equipmentChangeRequestValidator;
        private readonly ICustomValidator<CrewChangeRequest> _crewChangeRequestValidator;
        private readonly ICustomValidator<HearingRequest> _hearingRequestValidator;

        #region Constructors

        public RequestManager
            (
                IAuthorizationManager authorizationManager,
                IEquipmentChangeRequestRepository equipmentChangeRequestRepository,
                ICrewChangeRequestRepository crewChangeRequestRepository,
                IHearingRequestRepository hearingRequestRepository,
                IHearingRequestTypeRepository hearingRequestTypeRepository,
                IFileRepository fileRepository,
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


        private Color _COLOR_LIGHT_GREY = Colors.Grey.Lighten3;
        private Color _COLOR_GREY = Colors.Grey.Darken3;
        private Color _COLOR_WHITE = Colors.White;

        public void RenderHearingRequest(HearingRequest hearingRequest, ITransactionalContext? context = null)
        {
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

            const string filename = "C:\\FILES\\invoice.pdf";

            doc.GeneratePdf(filename);
        }

        #endregion

        #endregion

        private void AddHeader(IContainer container)
        {
            var defaultTextStyle = TextStyle.Default
            .FontFamily("Arial")
            .FontSize(7);

            IContainer CellStyleTopHeader(IContainer container) => container.DefaultTextStyle(defaultTextStyle);

            container.Column(column =>
            {
                column.Spacing(5);

                column.Item().Row(row =>
                {
                    row.ConstantItem(4.0f, Unit.Centimetre).Column(column =>
                    {
                        column.Item().Element(CellStyleTopHeader).Text("Recibido por oficial de regata:");
                    });

                    row.ConstantItem(3.0f, Unit.Centimetre).Column(column =>
                    {
                        column.Item().Element(CellStyleTopHeader).Text("N° Protesta . . . . . . . . . ");
                    });

                    row.ConstantItem(5.0f, Unit.Centimetre).Column(column =>
                    {
                        column.Item().Element(CellStyleTopHeader).Text("Fecha y hora . . . . . . . . . . . .  . . . . . . . . . . . . ");
                    });

                    row.ConstantItem(5.9f, Unit.Centimetre).Column(column =>
                    {
                        column.Item().Element(CellStyleTopHeader).Text("Firma . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . ");
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
                        column.Item().Text("FORMULARIO DE PROTESTA").FontSize(14).Bold();
                    });

                    row.RelativeItem(50).Column(column =>
                    {
                        column.Item().Text(" - también para pedidos de reparación y de reapertura").Bold().LineHeight(2.5f);
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
            IContainer CellStyleWithNoBorderAndTopPadding(IContainer container) => container.Border(0).PaddingTop(3).PaddingBottom(1).PaddingLeft(1).PaddingRight(1);
            IContainer CellStyleWithNoBorder(IContainer container) => container.Border(0).Padding(1);

            var defaultTextStyle = TextStyle.Default
                .FontColor(_COLOR_GREY);

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

                    table.Cell().Element(CellStyle).Text("Campeonato de Buenos Aires");
                    table.Cell().Element(CellStyle).Text("Club Náutico de San Isidro");
                    table.Cell().Element(CellStyle).Text("17/11/1983");
                    table.Cell().Element(CellStyle).Text("0015151");
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
                    table.Cell().Element(CellStyle).Text("370 Orca");
                    table.Cell().Element(CellStyle).Text("El nautiulus");
                    table.Cell().Element(CellStyle).Text("AR1201");

                    table.Cell().Element(CellStyleWithNoBorderAndTopPadding).Text("Representado por").Bold();
                    table.Cell().Element(CellStyleWithNoBorderAndTopPadding).Text("Domicilio").Bold();
                    table.Cell().Element(CellStyleWithNoBorderAndTopPadding).Text("Tel.").Bold();
                    table.Cell().Element(CellStyle).Text("Matías bello");
                    table.Cell().Element(CellStyle).Text("Lamadrid 249, piso 5, Bahía Blanca");
                    table.Cell().Element(CellStyle).Text("11 6715 1807");
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

                    for (int i = 0; i < 3; i++)
                    {
                        table.Cell().Element(CellStyleWithNoBorderAndTopPadding).Text("Clase").Bold();
                        table.Cell().Element(CellStyleWithNoBorderAndTopPadding).Text("Nombre del barco").Bold();
                        table.Cell().Element(CellStyleWithNoBorderAndTopPadding).Text("Vela N°").Bold();
                        table.Cell().Element(CellStyle).Text("370 Orca");
                        table.Cell().Element(CellStyle).Text("El nautiulus");
                        table.Cell().Element(CellStyle).Text("AR1201");
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
                    table.Cell().ColumnSpan(2).Element(CellStyle).Text("En la esquina de Segurola y Habana");

                    table.Cell().Element(CellStyleWithNoBorderAndTopPadding).Text("Reglas que se habrían infringido").Bold();
                    table.Cell().Element(CellStyleWithNoBorderAndTopPadding).Text("Testigos").Bold();
                    table.Cell().Element(CellStyle).Text("69.2, 103.1 bis, 99, y otras");
                    table.Cell().Element(CellStyle).Text("John Lennon, Paul McCartney, George Harrisson y Ringo Star");
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
                    table.Cell().RowSpan(2).AlignCenter().Text(GetCheckBoxSymbol(false)).FontSize(12);
                    table.Cell().Element(CellStyleWithNoBorderAndTopPadding).Text("Cuándo?").Bold();
                    table.Cell().Element(CellStyleWithNoBorderAndTopPadding).Text("Palabra(s) usada(s)").Bold();
                    table.Cell().Element(CellStyle).Text("Cuando me di cuenta");
                    table.Cell().Element(CellStyle).Text("Pará, hermano!!");

                    table.Cell().RowSpan(2).Element(CellStyleWithNoBorderAndTopPadding).Text("Desplegando una bandera roja").Bold();
                    table.Cell().RowSpan(2).AlignCenter().Text(GetCheckBoxSymbol(false)).FontSize(12);
                    table.Cell().ColumnSpan(2).Element(CellStyleWithNoBorderAndTopPadding).Text("Cuándo?").Bold();
                    table.Cell().ColumnSpan(2).Element(CellStyle).Text("No me acuerdo la verdad..");

                    table.Cell().RowSpan(2).Element(CellStyleWithNoBorderAndTopPadding).Text("Informándole de otro modo").Bold();
                    table.Cell().RowSpan(2).AlignCenter().Text(GetCheckBoxSymbol(false)).FontSize(12);
                    table.Cell().ColumnSpan(2).Element(CellStyleWithNoBorderAndTopPadding).Text("Aclare cómo").Bold();
                    table.Cell().ColumnSpan(2).Element(CellStyle).Text("Tomé un silbato y lo solplé, y también lancé una bengala");
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

                    table.Cell().Height(200).Background(_COLOR_LIGHT_GREY).Text(".").FontColor(_COLOR_LIGHT_GREY); //.Placeholder(".").;
                });
            });

        }

        private string GetCheckBoxSymbol(bool isChecked)
        {
            return isChecked ? "☑" : "☐";
        }
    }
}
