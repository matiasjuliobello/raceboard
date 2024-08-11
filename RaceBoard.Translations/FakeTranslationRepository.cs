using RaceBoard.Translations.Entities;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Translations
{
    public class FakeTranslationRepository : ITranslationRepository
    {
        private static class Languages
        {
            public const string English = "en-US";
            public const string Spanish = "es-AR";
        }

        #region Private Members

        private readonly List<Translation> _repository;

        #endregion

        #region ITranslationRepository implementation

        public List<Translation> Get(string? language = null)
        {
            return _repository;
        }

        public Translation Get(string key, string? language = null)
        {
            return _repository.FirstOrDefault(x => x.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase));
        }

        #endregion

        #region Constructors

        public FakeTranslationRepository()
        {
            _repository = new List<Translation>();

            AddHardcodedValues();
        }

        #endregion

        #region Private Methods

        private void AddHardcodedValues()
        {
            AddText(AddTranslation("UserPreferencesNotFound"),
                new Tuple<string, string>(Languages.Spanish, "No se encontró las preferencias del usuario"));
            AddText(AddTranslation("RequiredField"),
                new Tuple<string, string>(Languages.English, "Required field"),
                new Tuple<string, string>(Languages.Spanish, "Campo requerido"));
            AddText(AddTranslation("CouldNotSaveFile"),
                new Tuple<string, string>(Languages.English, "File could not be saved"),
                new Tuple<string, string>(Languages.Spanish, "No se pudo guardar el archivo"));
            AddText(AddTranslation("DeleteFailed"),
                new Tuple<string, string>(Languages.English, "Delete failed"),
                new Tuple<string, string>(Languages.Spanish, "No se pudo eliminar"));
            AddText(AddTranslation("RecordNotFound"),
                new Tuple<string, string>(Languages.English, "Record not found"),
                new Tuple<string, string>(Languages.Spanish, "No se encontró el registro"));
            AddText(AddTranslation("AnErrorOccurredWhileAttemptingToSaveData"),
                new Tuple<string, string>(Languages.English, "An error occurred while attempting to save data"),
                new Tuple<string, string>(Languages.Spanish, "Ocurrió un error al intentar guardar los datos"));
            AddText(AddTranslation("NeedPermissions"),
                new Tuple<string, string>(Languages.English, "User does not have permission to perform action"),
                new Tuple<string, string>(Languages.Spanish, "El usuario no tiene permiso para realizar la acción"));
            AddText(AddTranslation("IdStudioNotProvided"),
                new Tuple<string, string>(Languages.English, "ID studio was not provided"),
                new Tuple<string, string>(Languages.Spanish, "El ID de estudio no fue provisto"));
            AddText(AddTranslation("UserWasNotFound"),
                new Tuple<string, string>(Languages.English, "User was not found"),
                new Tuple<string, string>(Languages.Spanish, "El usuario no fue encontrado"));
            AddText(AddTranslation("InvalidCredentials"),
                new Tuple<string, string>(Languages.English, "Invalid credentials"),
                new Tuple<string, string>(Languages.Spanish, "Credenciales inválidas"));
            AddText(AddTranslation("IdUserIsRequired"),
                new Tuple<string, string>(Languages.English, "ID user is required"),
                new Tuple<string, string>(Languages.Spanish, "El ID de usuario es requerido"));
            AddText(AddTranslation("PasswordPolicyNotFound"),
                new Tuple<string, string>(Languages.English, "Password policy was not found"),
                new Tuple<string, string>(Languages.Spanish, "La política de contraseñas no fue encontada"));
            AddText(AddTranslation("PasswordIsRequired"),
                new Tuple<string, string>(Languages.English, "Password is required"),
                new Tuple<string, string>(Languages.Spanish, "La contraseña es requerido"));
            AddText(AddTranslation("PasswordMustHaveAtLeast_N_Characters"),
                new Tuple<string, string>(Languages.English, "Password must have at least {0} char(s) long"),
                new Tuple<string, string>(Languages.Spanish, "La contraseña debe tener al menos {0} caracteres de longitud"));
            AddText(AddTranslation("PasswordCannotHaveMoreThan_N_Characters"),
                new Tuple<string, string>(Languages.English, "Password cannot be more than {0} char(s) long"),
                new Tuple<string, string>(Languages.Spanish, "La contraseña no puede tener más de {0} caracter(es) de longitud"));
            AddText(AddTranslation("PasswordMustHaveAtLeast_N_NumericCharacters"),
                new Tuple<string, string>(Languages.English, "Password must have at least {0} number(s)"),
                new Tuple<string, string>(Languages.Spanish, "El password debe contener al menos {0} número(s)"));
            AddText(AddTranslation("PasswordMustHaveAtLeast_N_SpecialCharacters"),
                new Tuple<string, string>(Languages.English, "Password must have at least {0} special char(s)"),
                new Tuple<string, string>(Languages.Spanish, "El password debe contener al menos {0} caracter(es) especial(es)"));
            AddText(AddTranslation("PasswordMustHaveAtLeast_N_LowercaseCharacters"),
                new Tuple<string, string>(Languages.English, "Password must have at least {0} lowercase letter(s)"),
                new Tuple<string, string>(Languages.Spanish, "El password debe contener al menos {0} minúscula(s)"));
            AddText(AddTranslation("PasswordMustHaveAtLeast_N_UppercaseCharacters"),
                new Tuple<string, string>(Languages.English, "Password must have at least {0} uppercase letter(s)"),
                new Tuple<string, string>(Languages.Spanish, "El password debe contener al menos {0} mayúscula(s)"));
            AddText(AddTranslation("Welcome"),
                new Tuple<string, string>(Languages.Spanish, "Bienvenido"));
            AddText(AddTranslation("LogIn"),
                new Tuple<string, string>(Languages.Spanish, "Entrar"));
            AddText(AddTranslation("LogOut"),
                new Tuple<string, string>(Languages.Spanish, "Cerrar sesión"));
            AddText(AddTranslation("Username"),
                new Tuple<string, string>(Languages.Spanish, "Nombre de usuario"));
            AddText(AddTranslation("Password"),
                new Tuple<string, string>(Languages.Spanish, "Contraseña"));
            AddText(AddTranslation("ForgetData"),
                new Tuple<string, string>(Languages.Spanish, "Olvidó su contraseña?"));
            AddText(AddTranslation("PrivacyPolicy"),
                new Tuple<string, string>(Languages.Spanish, "Acuerdo de privacidad"));
            AddText(AddTranslation("DisclosureStatement"),
                new Tuple<string, string>(Languages.Spanish, "Política de divulgación"));
            AddText(AddTranslation("RecoverPassword"),
                new Tuple<string, string>(Languages.Spanish, "Recuperar contraseña"));
            AddText(AddTranslation("ContentTextRecoverPassword"),
                new Tuple<string, string>(Languages.Spanish, "Complete los datos para recibir un correo con instrucciones para reestablecer su contraseña"));
            AddText(AddTranslation("UserOrEmail"),
                new Tuple<string, string>(Languages.Spanish, "Nombre de usuario o email"));
            AddText(AddTranslation("Continue"),
                new Tuple<string, string>(Languages.Spanish, "Continuar"));
            AddText(AddTranslation("ChooseStudio"),
                new Tuple<string, string>(Languages.Spanish, "Seleccione el estudio"));
            AddText(AddTranslation("Studio"),
                new Tuple<string, string>(Languages.Spanish, "Estudio"));
            AddText(AddTranslation("Choose"),
                new Tuple<string, string>(Languages.Spanish, "Seleccionar"));
            AddText(AddTranslation("CreatedSuccessfully"),
                new Tuple<string, string>(Languages.Spanish, "Creado exitosamente"));
            AddText(AddTranslation("ModifiedSuccessfully"),
                new Tuple<string, string>(Languages.Spanish, "Modificado exitosamente"));
            AddText(AddTranslation("DeletedSuccessfully"),
                new Tuple<string, string>(Languages.Spanish, "Eliminado exitosamente"));
            AddText(AddTranslation("SavedSuccessfully"),
                new Tuple<string, string>(Languages.Spanish, "Guardado exitosamente"));
            AddText(AddTranslation("StudioManagement"),
                new Tuple<string, string>(Languages.Spanish, "Administración del estudio"));
            AddText(AddTranslation("Projects"),
                new Tuple<string, string>(Languages.Spanish, "Proyectos"));
            AddText(AddTranslation("Scripts"),
                new Tuple<string, string>(Languages.Spanish, "Guiones"));
            AddText(AddTranslation("Script"),
                new Tuple<string, string>(Languages.Spanish, "Guión"));
            AddText(AddTranslation("Create"),
                new Tuple<string, string>(Languages.Spanish, "Crear"));
            AddText(AddTranslation("Edit"),
                new Tuple<string, string>(Languages.Spanish, "Editar"));
            AddText(AddTranslation("Delete"),
                new Tuple<string, string>(Languages.Spanish, "Eliminar"));
            AddText(AddTranslation("Cancel"),
                new Tuple<string, string>(Languages.Spanish, "Cancelar"));
            AddText(AddTranslation("Save"),
                new Tuple<string, string>(Languages.Spanish, "Guardar"));
            AddText(AddTranslation("SearchBy"),
                new Tuple<string, string>(Languages.Spanish, "Buscar por"));
            AddText(AddTranslation("Id"),
                new Tuple<string, string>(Languages.Spanish, "Id"));
            AddText(AddTranslation("Name"),
                new Tuple<string, string>(Languages.Spanish, "Nombre"));
            AddText(AddTranslation("Type"),
                new Tuple<string, string>(Languages.Spanish, "Tipo"));
            AddText(AddTranslation("CreationDate"),
                new Tuple<string, string>(Languages.Spanish, "Fecha de creación"));
            AddText(AddTranslation("Title"),
                new Tuple<string, string>(Languages.Spanish, "Título"));
            AddText(AddTranslation("Episode"),
                new Tuple<string, string>(Languages.Spanish, "Episodio"));
            AddText(AddTranslation("Project"),
                new Tuple<string, string>(Languages.Spanish, "Proyecto"));
            AddText(AddTranslation("Status"),
                new Tuple<string, string>(Languages.Spanish, "Estado"));
            AddText(AddTranslation("RunningTime"),
                new Tuple<string, string>(Languages.Spanish, "Duración"));
            AddText(AddTranslation("Pages"),
                new Tuple<string, string>(Languages.Spanish, "Páginas"));
            AddText(AddTranslation("Loops"),
                new Tuple<string, string>(Languages.Spanish, "Loops"));
            AddText(AddTranslation("HasImport"),
                new Tuple<string, string>(Languages.Spanish, "Importación"));
            AddText(AddTranslation("HasPendingApproval"),
                new Tuple<string, string>(Languages.Spanish, "Aprobación pendiente"));
            AddText(AddTranslation("FromForm"),
                new Tuple<string, string>(Languages.Spanish, "Desde formulario"));
            AddText(AddTranslation("FromImport"),
                new Tuple<string, string>(Languages.Spanish, "Desde importación"));
            AddText(AddTranslation("ChooseHowToCreateScript"),
                new Tuple<string, string>(Languages.Spanish, "Cómo desea crear el guión?"));
            AddText(AddTranslation("NoFileWasSelected"),
                new Tuple<string, string>(Languages.Spanish, "No se seleccionó ningún archivo"));
            AddText(AddTranslation("NoScriptWasSelected"),
                new Tuple<string, string>(Languages.Spanish, "No se seleccionó ningún guión"));
            AddText(AddTranslation("NoProjectWasSelected"),
                new Tuple<string, string>(Languages.Spanish, "No se seleccionó ningún proyecto"));
            AddText(AddTranslation("NoTranslatorWasSelected"),
                new Tuple<string, string>(Languages.Spanish, "No se seleccionó ningún traductor"));
            AddText(AddTranslation("ViewScripts"),
                new Tuple<string, string>(Languages.Spanish, "Ver guiones"));
            AddText(AddTranslation("AddImportedScript"),
                new Tuple<string, string>(Languages.Spanish, "Agregar importación"));
            AddText(AddTranslation("Projects"),
                new Tuple<string, string>(Languages.Spanish, "Proyectos"));
            AddText(AddTranslation("Translator"),
                new Tuple<string, string>(Languages.Spanish, "Traductor"));
            AddText(AddTranslation("SkipFirstRow"),
                new Tuple<string, string>(Languages.Spanish, "Omitir primer fila"));
            AddText(AddTranslation("DragAndDropFileHere"),
                new Tuple<string, string>(Languages.Spanish, "Arrastrar y soltar archivos aquí"));
            AddText(AddTranslation("OrYouCan"),
                new Tuple<string, string>(Languages.Spanish, "o también puedes"));
            AddText(AddTranslation("UploadFile"),
                new Tuple<string, string>(Languages.Spanish, "Seleccionar archivo"));
            AddText(AddTranslation("AcceptedUploadFileFormats"),
                new Tuple<string, string>(Languages.Spanish, "Formatos de archivo permitidos"));
            AddText(AddTranslation("OnlyOneFileCanBeUploaded"),
                new Tuple<string, string>(Languages.Spanish, "Sólo se puede subir un archivo"));
            AddText(AddTranslation("DataFileTypes"),
                new Tuple<string, string>(Languages.Spanish, "Tipos de archivo"));
            AddText(AddTranslation("FileToProcess"),
                new Tuple<string, string>(Languages.Spanish, "Archivo a procesar"));
            AddText(AddTranslation("CreateProject"),
                new Tuple<string, string>(Languages.Spanish, "Crear proyecto"));
            AddText(AddTranslation("CreateScript"),
                new Tuple<string, string>(Languages.Spanish, "Crear guión"));
            AddText(AddTranslation("ProjectType"),
                new Tuple<string, string>(Languages.Spanish, "Tipo de Proyecto"));
            AddText(AddTranslation("Projects"),
                new Tuple<string, string>(Languages.Spanish, "Proyectos"));
            AddText(AddTranslation("Statuses"),
                new Tuple<string, string>(Languages.Spanish, "Estados"));
            AddText(AddTranslation("RoleAssignment"),
                new Tuple<string, string>(Languages.Spanish, "Asignación de roles"));
            AddText(AddTranslation("AddRoles"),
                new Tuple<string, string>(Languages.Spanish, "Agregar roles"));
            AddText(AddTranslation("Manager"),
                new Tuple<string, string>(Languages.Spanish, "Administrador"));
            AddText(AddTranslation("Translator"),
                new Tuple<string, string>(Languages.Spanish, "Traductor"));
            AddText(AddTranslation("Director"),
                new Tuple<string, string>(Languages.Spanish, "Director"));
            AddText(AddTranslation("Adapter"),
                new Tuple<string, string>(Languages.Spanish, "Adaptador"));
            AddText(AddTranslation("Operator"),
                new Tuple<string, string>(Languages.Spanish, "Operador"));
            AddText(AddTranslation("Editor"),
                new Tuple<string, string>(Languages.Spanish, "Editor"));
            AddText(AddTranslation("Mixer"),
                new Tuple<string, string>(Languages.Spanish, "Mezclador"));
            AddText(AddTranslation("Producer"),
                new Tuple<string, string>(Languages.Spanish, "Productor"));
            AddText(AddTranslation("Timer"),
                new Tuple<string, string>(Languages.Spanish, "Temporizador"));
            AddText(AddTranslation("Characters"),
                new Tuple<string, string>(Languages.Spanish, "Personajes"));
            AddText(AddTranslation("AddCharacters"),
                new Tuple<string, string>(Languages.Spanish, "Agregar personajes"));
            AddText(AddTranslation("AddCharacter"),
                new Tuple<string, string>(Languages.Spanish, "Agregar personaje"));
            AddText(AddTranslation("Character"),
                new Tuple<string, string>(Languages.Spanish, "Personaje"));
            AddText(AddTranslation("Actor"),
                new Tuple<string, string>(Languages.Spanish, "Actor"));
            AddText(AddTranslation("RecordingDate"),
                new Tuple<string, string>(Languages.Spanish, "Fecha de grabación"));
            AddText(AddTranslation("RecodingType"),
                new Tuple<string, string>(Languages.Spanish, "Tipo de grabación"));
            AddText(AddTranslation("Cost"),
                new Tuple<string, string>(Languages.Spanish, "Costo"));
            AddText(AddTranslation("MinLoopsAmount"),
                new Tuple<string, string>(Languages.Spanish, "Cantidad mínima de loops"));
            AddText(AddTranslation("MinLoopsValue"),
                new Tuple<string, string>(Languages.Spanish, "Valor mínimo de loop"));
            AddText(AddTranslation("LoopUnitValue"),
                new Tuple<string, string>(Languages.Spanish, "Valor unitario de loop"));
            AddText(AddTranslation("ChorusUnitValue"),
                new Tuple<string, string>(Languages.Spanish, "Valor unitario de coro"));
            AddText(AddTranslation("WordUnitValue"),                 
                new Tuple<string, string>(Languages.Spanish, "Valor unitario de palabra"));
            AddText(AddTranslation("DirectorFactor"),                 
                new Tuple<string, string>(Languages.Spanish, "Factor de dirección"));
            AddText(AddTranslation("TranslationFactor"),
                new Tuple<string, string>(Languages.Spanish, "Factor de traducción"));
            AddText(AddTranslation("AdaptationFactor"),
                new Tuple<string, string>(Languages.Spanish, "Factor de adaptación"));
            AddText(AddTranslation("MixFactor"),
                new Tuple<string, string>(Languages.Spanish, "Factor de mezcla"));
            AddText(AddTranslation("TimingFactor"),
                new Tuple<string, string>(Languages.Spanish, "Factor de temporización"));
            AddText(AddTranslation("Description"),
                new Tuple<string, string>(Languages.Spanish, "Descripción"));
            AddText(AddTranslation("AddDescription"),
                new Tuple<string, string>(Languages.Spanish, "Agregar descripción"));
            AddText(AddTranslation("Override"),
                new Tuple<string, string>(Languages.Spanish, "Sobreescribir"));
            AddText(AddTranslation("Import"),
                new Tuple<string, string>(Languages.Spanish, "Importación"));
            AddText(AddTranslation("ImportData"),
                new Tuple<string, string>(Languages.Spanish, "Datos de importación"));
            AddText(AddTranslation("User"),
                new Tuple<string, string>(Languages.Spanish, "Usuario"));
            AddText(AddTranslation("ImportDate"),
                new Tuple<string, string>(Languages.Spanish, "Fecha de importación"));
            AddText(AddTranslation("Approval"),
                new Tuple<string, string>(Languages.Spanish, "Aprobación"));
            AddText(AddTranslation("ApprovalData"),
                new Tuple<string, string>(Languages.Spanish, "Datos de aprobación"));
            AddText(AddTranslation("Hiring"),
                new Tuple<string, string>(Languages.Spanish, "Contración"));
            AddText(AddTranslation("Hirings"),
                new Tuple<string, string>(Languages.Spanish, "Contraciones"));
            AddText(AddTranslation("Filter"),
                new Tuple<string, string>(Languages.Spanish, "Filtro"));
            AddText(AddTranslation("FullName"),
                new Tuple<string, string>(Languages.Spanish, "Nombre completo"));
            AddText(AddTranslation("HiringTypes"),
                new Tuple<string, string>(Languages.Spanish, "Tipos de contratación"));
            AddText(AddTranslation("Roles"),
                new Tuple<string, string>(Languages.Spanish, "Roles"));
            AddText(AddTranslation("Role"),
                new Tuple<string, string>(Languages.Spanish, "Rol"));
            AddText(AddTranslation("StartDate"),
                new Tuple<string, string>(Languages.Spanish, "Fecha de inicio"));
            AddText(AddTranslation("EndDate"),
                new Tuple<string, string>(Languages.Spanish, "Fecha de fin"));
            AddText(AddTranslation("AddHiring"),
                new Tuple<string, string>(Languages.Spanish, "Agregar contratación"));
            AddText(AddTranslation("UserData"),
                new Tuple<string, string>(Languages.Spanish, "Datos del usuario"));
            AddText(AddTranslation("FirstName"),
                new Tuple<string, string>(Languages.Spanish, "Nombre"));
            AddText(AddTranslation("MiddleName"),
                new Tuple<string, string>(Languages.Spanish, "Segundo nombre"));
            AddText(AddTranslation("LastName"),
                new Tuple<string, string>(Languages.Spanish, "Apellido"));
            AddText(AddTranslation("BirthDate"),
                new Tuple<string, string>(Languages.Spanish, "Fecha de nacimiento"));
            AddText(AddTranslation("Email"),
                new Tuple<string, string>(Languages.Spanish, "Email"));
            AddText(AddTranslation("IsActive"),
                new Tuple<string, string>(Languages.Spanish, "Activo"));
            AddText(AddTranslation("HiringData"),
                new Tuple<string, string>(Languages.Spanish, "Datos de contratación"));
            AddText(AddTranslation("HiringType"),
                new Tuple<string, string>(Languages.Spanish, "Tipo de contratación"));
            AddText(AddTranslation("AddUser"),
                new Tuple<string, string>(Languages.Spanish, "Agregar usuario"));
            AddText(AddTranslation("Apply"),
                new Tuple<string, string>(Languages.Spanish, "Aplicar"));
            AddText(AddTranslation("Payments"),
                new Tuple<string, string>(Languages.Spanish, "Pagos"));
            AddText(AddTranslation("StudioDataManagement"),
                new Tuple<string, string>(Languages.Spanish, "Administración de datos del estudio"));
            AddText(AddTranslation("LegalRepresentative"),
                new Tuple<string, string>(Languages.Spanish, "Responsable legal"));
            AddText(AddTranslation("ContactFullName"),
                new Tuple<string, string>(Languages.Spanish, "Nombre completo del contacto"));
            AddText(AddTranslation("PhoneNumber"),
                new Tuple<string, string>(Languages.Spanish, "Número de teléfono"));
            AddText(AddTranslation("CellNumber"),
                new Tuple<string, string>(Languages.Spanish, "Número de celular"));
            AddText(AddTranslation("EmailAddress"),
                new Tuple<string, string>(Languages.Spanish, "Casilla de email"));
            AddText(AddTranslation("CompanyName"),
                new Tuple<string, string>(Languages.Spanish, "Razón social"));
            AddText(AddTranslation("TaxIdentificationNumber"),
                new Tuple<string, string>(Languages.Spanish, "CUIT"));
            AddText(AddTranslation("GrossIncomeTax"),
                new Tuple<string, string>(Languages.Spanish, "Ingresos brutos"));
            AddText(AddTranslation("FullAddress"),
                new Tuple<string, string>(Languages.Spanish, "Domicilio completo"));
            AddText(AddTranslation("DuplicateRecordExists"),
                new Tuple<string, string>(Languages.Spanish, "Ya existe un registro con los mismos datos."));
            AddText(AddTranslation("AnIdentificationOfSameTypeAlreadyExistsForThisUser"),
                new Tuple<string, string>(Languages.Spanish, "Ya existe un documento del mismo tipo para este usuario."));
            AddText(AddTranslation("Timesheets"),
                new Tuple<string, string>(Languages.Spanish, "Planillas"));
            AddText(AddTranslation("Payroll"),
                new Tuple<string, string>(Languages.Spanish, "Liquidación"));
            AddText(AddTranslation("Billing"),
                new Tuple<string, string>(Languages.Spanish, "Facturación"));
            AddText(AddTranslation("Actors"),
                new Tuple<string, string>(Languages.Spanish, "Actores"));
            AddText(AddTranslation("Staff"),
                new Tuple<string, string>(Languages.Spanish, "Staff"));
            AddText(AddTranslation("Credits"),
                new Tuple<string, string>(Languages.Spanish, "Créditos"));
            AddText(AddTranslation("PaymentMethod"),
                new Tuple<string, string>(Languages.Spanish, "Método de pago"));
            AddText(AddTranslation("LegalOwner"),
                new Tuple<string, string>(Languages.Spanish, "Representante legal"));
            AddText(AddTranslation("LegalAddress"),
                new Tuple<string, string>(Languages.Spanish, "Domicilio legal"));
            AddText(AddTranslation("RealAddress"),
                new Tuple<string, string>(Languages.Spanish, "Domicilio real"));
            AddText(AddTranslation("Phone"),
                new Tuple<string, string>(Languages.Spanish, "Teléfono"));
            AddText(AddTranslation("ContactName"),
                new Tuple<string, string>(Languages.Spanish, "Nombre de contacto"));
            AddText(AddTranslation("ContactPhone"),
                new Tuple<string, string>(Languages.Spanish, "Teléfono de contacto"));
            AddText(AddTranslation("ContactCellPhone"),
                new Tuple<string, string>(Languages.Spanish, "Celular de contacto"));
            AddText(AddTranslation("ContactEmail"),
                new Tuple<string, string>(Languages.Spanish, "Email de contacto"));
            AddText(AddTranslation("GrossIncome"),
                new Tuple<string, string>(Languages.Spanish, "Ingresos brutos"));
            AddText(AddTranslation("Program"),
                new Tuple<string, string>(Languages.Spanish, "Programa"));
            AddText(AddTranslation("Author"),
                new Tuple<string, string>(Languages.Spanish, "Autor"));
            AddText(AddTranslation("PayrollPeriod"),
                new Tuple<string, string>(Languages.Spanish, "Período de liquidación"));
            AddText(AddTranslation("Program"),
                new Tuple<string, string>(Languages.Spanish, "Programa"));
            AddText(AddTranslation("StartDate"),
                new Tuple<string, string>(Languages.Spanish, "Fecha desde"));
            AddText(AddTranslation("EndDate"),
                new Tuple<string, string>(Languages.Spanish, "Fecha hasta"));
            AddText(AddTranslation("DocumentNumber"),
                new Tuple<string, string>(Languages.Spanish, "Número de documento"));
            AddText(AddTranslation("DocumentType"),
                new Tuple<string, string>(Languages.Spanish, "Tipo de documento"));
            AddText(AddTranslation("CollaborationDate"),
                new Tuple<string, string>(Languages.Spanish, "Fecha colaboración"));
            AddText(AddTranslation("EpisodeNumber"),
                new Tuple<string, string>(Languages.Spanish, "Capítulo"));
            AddText(AddTranslation("Contract"),
                new Tuple<string, string>(Languages.Spanish, "Contrato"));
            AddText(AddTranslation("Bolus"),
                new Tuple<string, string>(Languages.Spanish, "Bolos"));
            AddText(AddTranslation("Outdoors"),
                new Tuple<string, string>(Languages.Spanish, "Exteriores"));
            AddText(AddTranslation("Surpluses"),
                new Tuple<string, string>(Languages.Spanish, "Excedentes"));
            AddText(AddTranslation("Reshoots"),
                new Tuple<string, string>(Languages.Spanish, "Repetición"));
            AddText(AddTranslation("Other"),
                new Tuple<string, string>(Languages.Spanish, "Otros"));
            AddText(AddTranslation("TotalGrossRemuneration"),
                new Tuple<string, string>(Languages.Spanish, "Remun. Total Bruta"));
            AddText(AddTranslation("PerDiem"),
                new Tuple<string, string>(Languages.Spanish, "Viáticos"));
            AddText(AddTranslation("Voucher"),
                new Tuple<string, string>(Languages.Spanish, "Vales"));
            AddText(AddTranslation("Details"),
                new Tuple<string, string>(Languages.Spanish, "Detalles"));
            AddText(AddTranslation("Observations"),
                new Tuple<string, string>(Languages.Spanish, "Observaciones"));
            AddText(AddTranslation("TotalHealthInsuranceContribution"),
                new Tuple<string, string>(Languages.Spanish, "Total contribución OSA"));
            AddText(AddTranslation("TotalUnionContribution"),
                new Tuple<string, string>(Languages.Spanish, "Total contribución sindicato"));
            AddText(AddTranslation("TotalGeneralRemunerations"),
                new Tuple<string, string>(Languages.Spanish, "Total general remuneraciones"));
            AddText(AddTranslation("TotalDeposit"),
                new Tuple<string, string>(Languages.Spanish, "Total depósito"));
            AddText(AddTranslation("NetTotal"),
                new Tuple<string, string>(Languages.Spanish, "Total neto"));
            AddText(AddTranslation("GenerateSpreadsheet"),
                new Tuple<string, string>(Languages.Spanish, "Generar planilla"));
            AddText(AddTranslation("AddToSpreadsheet"),
                new Tuple<string, string>(Languages.Spanish, "Agregar a planilla"));
            AddText(AddTranslation("ChooseTimesheetToCreate"),
                new Tuple<string, string>(Languages.Spanish, "Qué tipo de planilla desea crear?"));
            AddText(AddTranslation("LicenseNumber"),
                new Tuple<string, string>(Languages.Spanish, "Número de matrícula"));
            AddText(AddTranslation("PaymentDate"),
                new Tuple<string, string>(Languages.Spanish, "Fecha de pago"));
            AddText(AddTranslation("Provider"),
                new Tuple<string, string>(Languages.Spanish, "Proveedor"));
            AddText(AddTranslation("Amount"),
                new Tuple<string, string>(Languages.Spanish, "Importe"));
            AddText(AddTranslation("PaymentMethod"),
                new Tuple<string, string>(Languages.Spanish, "Método de pago"));
            AddText(AddTranslation("PendingPaymentPayments"),
                new Tuple<string, string>(Languages.Spanish, "Pendientes de pago"));
            AddText(AddTranslation("PaidPayments"),
                new Tuple<string, string>(Languages.Spanish, "Pagados"));
            AddText(AddTranslation("Spendings"),
                new Tuple<string, string>(Languages.Spanish, "Gastos"));
            AddText(AddTranslation("Date"),
                new Tuple<string, string>(Languages.Spanish, "Fecha"));
            AddText(AddTranslation("Period"),
                new Tuple<string, string>(Languages.Spanish, "Período"));
            AddText(AddTranslation("Item"),
                new Tuple<string, string>(Languages.Spanish, "Ítem"));
            AddText(AddTranslation("Category"),
                new Tuple<string, string>(Languages.Spanish, "Categoría"));
            AddText(AddTranslation("IdActionCannotBeRepeated"),
                new Tuple<string, string>(Languages.Spanish, "La acción no puede estar repetida"));
        }

        private Translation AddTranslation(string key)
        {
            var translation = new Translation()
            {
                Key = key,
                Translations = new List<TranslatedText>()
            };

            _repository.Add(translation);

            return translation;
        }

        private void AddText(Translation translation, params Tuple<string, string>[] translatedTexts)
        {
            foreach(var t in translatedTexts)
            {
                translation.Translations.Add(new TranslatedText() { Language = t.Item1, Text = t.Item2 } );
            }
        }

        #endregion
    }
}
