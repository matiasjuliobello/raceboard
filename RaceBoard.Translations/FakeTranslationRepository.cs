using Newtonsoft.Json.Linq;
using RaceBoard.Translations.Entities;
using RaceBoard.Translations.Interfaces;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Xml;

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
            AddText(AddTranslation("RecordNotFound"),
                new Tuple<string, string>(Languages.English, "Record not found"),
                new Tuple<string, string>(Languages.Spanish, "No se encontró el registro"));
            AddText(AddTranslation("Oops!LooksLikeYouAreLost"),
                new Tuple<string, string>(Languages.English, "Oops! Looks like you're lost.."),
                new Tuple<string, string>(Languages.Spanish, "Oops! Parece que estás perdido.."));
            AddText(AddTranslation("Oops!AnErrorHasOccurred"),
                new Tuple<string, string>(Languages.English, "Oops! An error has occurred"),
                new Tuple<string, string>(Languages.Spanish, "Oops! Ocurrió un error.."));
            AddText(AddTranslation("TheDataYouAreLookingForWasNotFound"),
                new Tuple<string, string>(Languages.English, "The data you are looking for was not found"),
                new Tuple<string, string>(Languages.Spanish, "No se encontraron los datos que estás buscando"));

            AddText(AddTranslation("MySettings"),
                new Tuple<string, string>(Languages.English, "My settings"),
                new Tuple<string, string>(Languages.Spanish, "Mi configuración"));
            AddText(AddTranslation("PersonalData"),
                new Tuple<string, string>(Languages.English, "Personal data"),
                new Tuple<string, string>(Languages.Spanish, "Datos personales"));
            AddText(AddTranslation("Preferences"),
                new Tuple<string, string>(Languages.English, "Preferences"),
                new Tuple<string, string>(Languages.Spanish, "Preferencias"));
            AddText(AddTranslation("Logout"),
                new Tuple<string, string>(Languages.English, "Logout"),
                new Tuple<string, string>(Languages.Spanish, "Cerrar sesión"));

            AddText(AddTranslation("Language"),
                new Tuple<string, string>(Languages.English, "Language"),
                new Tuple<string, string>(Languages.Spanish, "Idioma"));
            AddText(AddTranslation("DateFormat"),
                new Tuple<string, string>(Languages.English, "Date format"),
                new Tuple<string, string>(Languages.Spanish, "Formato de fecha"));
            AddText(AddTranslation("TimeZone"),
                new Tuple<string, string>(Languages.English, "Time zone"),
                new Tuple<string, string>(Languages.Spanish, "Zona horaria"));
            AddText(AddTranslation("Users"),
                new Tuple<string, string>(Languages.English, "Users"),
                new Tuple<string, string>(Languages.Spanish, "Usuarios"));
            AddText(AddTranslation("Users"),
                new Tuple<string, string>(Languages.English, "Users"),
                new Tuple<string, string>(Languages.Spanish, "Usuarios"));
            AddText(AddTranslation("SavePreferences"),
                new Tuple<string, string>(Languages.English, "Save preferences"),
                new Tuple<string, string>(Languages.Spanish, "Guardar preferencias"));
            AddText(AddTranslation("ChangePreferences"),
                new Tuple<string, string>(Languages.English, "Change preferences"),
                new Tuple<string, string>(Languages.Spanish, "Cambiar preferencias"));
            AddText(AddTranslation("SavePreferences"),
                new Tuple<string, string>(Languages.English, "Save preferences"),
                new Tuple<string, string>(Languages.Spanish, "Guardar preferencias"));
            AddText(AddTranslation("Hello!Welcome"),
                new Tuple<string, string>(Languages.English, "Hello! Welcome"),
                new Tuple<string, string>(Languages.Spanish, "Hola! Bienvenido"));
            AddText(AddTranslation("LoadingTimeZones.."),
                new Tuple<string, string>(Languages.English, "Loading time zones.."),
                new Tuple<string, string>(Languages.Spanish, "Cargando zonas horarias.."));
            AddText(AddTranslation("LoadingLanguages.."),
                new Tuple<string, string>(Languages.English, "Loading languages.."),
                new Tuple<string, string>(Languages.Spanish, "Cargando idiomas."));

            AddText(AddTranslation("Main"),
                new Tuple<string, string>(Languages.English, "Main"),
                new Tuple<string, string>(Languages.Spanish, "Principal"));

            AddText(AddTranslation("LoadingCountries.."),
                new Tuple<string, string>(Languages.English, "Loading countries.."),
                new Tuple<string, string>(Languages.Spanish, "Cargando países.."));
            AddText(AddTranslation("LoadingBloodTypes.."),
                new Tuple<string, string>(Languages.English, "Loading blood types.."),
                new Tuple<string, string>(Languages.Spanish, "Cargando tipos de sangre.."));
            AddText(AddTranslation("LoadingMedicalInsurances.."),
                new Tuple<string, string>(Languages.English, "Loading medical insurances.."),
                new Tuple<string, string>(Languages.Spanish, "Cargando obras sociales.."));
            AddText(AddTranslation("LoadingMembers.."),
                new Tuple<string, string>(Languages.English, "Loading members.."),
                new Tuple<string, string>(Languages.Spanish, "Cargando integrantes.."));
            AddText(AddTranslation("LoadingRaceCategories.."),
                new Tuple<string, string>(Languages.English, "Loading categories.."),
                new Tuple<string, string>(Languages.Spanish, "Cargando categorías.."));
            AddText(AddTranslation("LoadingRaceClasses.."),
                new Tuple<string, string>(Languages.English, "Loading classes.."),
                new Tuple<string, string>(Languages.Spanish, "Cargando clases.."));
            AddText(AddTranslation("LoadingRoles.."),
                new Tuple<string, string>(Languages.English, "Loading roles.."),
                new Tuple<string, string>(Languages.Spanish, "Cargando roles.."));

            AddText(AddTranslation("DataSaved"),
                new Tuple<string, string>(Languages.English, "Data saved"),
                new Tuple<string, string>(Languages.Spanish, "Datos guardados"));

            AddText(AddTranslation("Success"),
                new Tuple<string, string>(Languages.English, "Success"),
                new Tuple<string, string>(Languages.Spanish, "Operación exitosa"));
            AddText(AddTranslation("ValidationError"),
                new Tuple<string, string>(Languages.English, "Validation error"),
                new Tuple<string, string>(Languages.Spanish, "Error de validación"));
            AddText(AddTranslation("GeneralError"),
                new Tuple<string, string>(Languages.English, "General error"),
                new Tuple<string, string>(Languages.Spanish, "Error general"));

            AddText(AddTranslation("Registration"),
                new Tuple<string, string>(Languages.English, "Registration"),
                new Tuple<string, string>(Languages.Spanish, "Inscripción"));
            AddText(AddTranslation("Accreditation"),
                new Tuple<string, string>(Languages.English, "Accreditation"),
                new Tuple<string, string>(Languages.Spanish, "Acreditación"));

            AddText(AddTranslation("DatesHaveNotBeenSetYet"),
                new Tuple<string, string>(Languages.English, "Dates have not been set yet"),
                new Tuple<string, string>(Languages.Spanish, "Aún no se han fijado las fechas"));
            

            AddText(AddTranslation("Pending"),
                new Tuple<string, string>(Languages.English, "Pending"),
                new Tuple<string, string>(Languages.Spanish, "Pendiente"));
            AddText(AddTranslation("Confirmed"),
                new Tuple<string, string>(Languages.English, "Confirmed"),
                new Tuple<string, string>(Languages.Spanish, "Confirmado"));
            AddText(AddTranslation("Removed"),
                new Tuple<string, string>(Languages.English, "Removed"),
                new Tuple<string, string>(Languages.Spanish, "Eliminado"));

            AddText(AddTranslation("Invite"),
                new Tuple<string, string>(Languages.English, "Invite"),
                new Tuple<string, string>(Languages.Spanish, "Invitar"));
            AddText(AddTranslation("InvitePersonToJoin"),
                new Tuple<string, string>(Languages.English, "Invite person to join"),
                new Tuple<string, string>(Languages.Spanish, "Invitar persona a unirse"));
            AddText(AddTranslation("InvitationExpired"),
                new Tuple<string, string>(Languages.English, "Invitation expired"),
                new Tuple<string, string>(Languages.Spanish, "Invitación vencida"));
            AddText(AddTranslation("InvitationWasSent"),
                new Tuple<string, string>(Languages.English, "Invitation was sent"),
                new Tuple<string, string>(Languages.Spanish, "Se envió la invitación"));

            AddText(AddTranslation("Members"),
                new Tuple<string, string>(Languages.English, "Members"),
                new Tuple<string, string>(Languages.Spanish, "Integrantes"));
            AddText(AddTranslation("MemberAdded"),
                new Tuple<string, string>(Languages.English, "Member added"),
                new Tuple<string, string>(Languages.Spanish, "Integrante agregado"));
            AddText(AddTranslation("NoMembersAddedYet"),
                new Tuple<string, string>(Languages.English, "No members added yet"),
                new Tuple<string, string>(Languages.Spanish, "Aún no se agregaron integrantes"));
            AddText(AddTranslation("MemberWasInvited"),
                new Tuple<string, string>(Languages.English, "Invitation was sent to member"),
                new Tuple<string, string>(Languages.Spanish, "Se envió la invitación al integrante"));
            AddText(AddTranslation("RemoveMember"),
                new Tuple<string, string>(Languages.English, "Remove member"),
                new Tuple<string, string>(Languages.Spanish, "Eliminar integrante"));
            AddText(AddTranslation("MemberWasRemoved"),
                new Tuple<string, string>(Languages.English, "Member was removed"),
                new Tuple<string, string>(Languages.Spanish, "Se eliminó al integrante"));

            AddText(AddTranslation("MemberHasNotConfirmedParticipationYet"),
                new Tuple<string, string>(Languages.English, "Member has not confirmed participation yet"),
                new Tuple<string, string>(Languages.Spanish, "El integrante aún no confirmó su participación"));
            AddText(AddTranslation("MemberHasConfirmedParticipation"),
                new Tuple<string, string>(Languages.English, "Member has confirmed participation"),
                new Tuple<string, string>(Languages.Spanish, "El integrante confirmó su participación"));
            AddText(AddTranslation("MemberAccessHasBeenRevoked"),
                new Tuple<string, string>(Languages.English, "Member access has been revoked"),
                new Tuple<string, string>(Languages.Spanish, "Se ha revocado el acceso al integrante"));

            AddText(AddTranslation("UserPreferencesNotFound"),
                new Tuple<string, string>(Languages.English, "User preferences not found"),
                new Tuple<string, string>(Languages.Spanish, "No se encontró las preferencias del usuario"));
            AddText(AddTranslation("DuplicateRecordExists"),
                new Tuple<string, string>(Languages.English, "Looks there's already a record with same data"),
                new Tuple<string, string>(Languages.Spanish, "Parece que ya existe un registro con los mismos datos"));
            AddText(AddTranslation("CannotDeleteMemberDueToExistingParticipation"),
                new Tuple<string, string>(Languages.English, "Could not delete member due to existing participations with team"),
                new Tuple<string, string>(Languages.Spanish, "No se puede remover al integrante porque ya tuvo participaciones con en el equipo"));
            AddText(AddTranslation("BoatAlreadyAssignedToAnotherTeam"),
                new Tuple<string, string>(Languages.English, "The boat is already assigned to another team in the same championship"),
                new Tuple<string, string>(Languages.Spanish, "El barco ya está asignado a otro equipo de la misma campeonato"));

            AddText(AddTranslation("OrganizationWasCreated"),
                new Tuple<string, string>(Languages.English, "Club was created"),
                new Tuple<string, string>(Languages.Spanish, "Se creó el club"));
            AddText(AddTranslation("ChampionshipWasCreated"),
                new Tuple<string, string>(Languages.English, "Championship was created"),
                new Tuple<string, string>(Languages.Spanish, "Se creó el campeonato"));
            AddText(AddTranslation("TeamWasCreated"),
                new Tuple<string, string>(Languages.English, "Team was created"),
                new Tuple<string, string>(Languages.Spanish, "Se creó el equipo"));
            AddText(AddTranslation("TeamWasDeleted"),
                new Tuple<string, string>(Languages.English, "Team was deleted"),
                new Tuple<string, string>(Languages.Spanish, "Se eliminó el equipo"));
            AddText(AddTranslation("GroupWasRemoved"),
                new Tuple<string, string>(Languages.English, "Group was removed"),
                new Tuple<string, string>(Languages.Spanish, "Se eliminó al grupo"));
            AddText(AddTranslation("FileWasRemoved"),
                new Tuple<string, string>(Languages.English, "File was removed"),
                new Tuple<string, string>(Languages.Spanish, "Se eliminó al archivo"));
            AddText(AddTranslation("FlagWasRemoved"),
                new Tuple<string, string>(Languages.English, "Flag was removed from"),
                new Tuple<string, string>(Languages.Spanish, "Se eliminó la bandera"));
            AddText(AddTranslation("BoatWasRemoved"),
                new Tuple<string, string>(Languages.English, "Boat was removed"),
                new Tuple<string, string>(Languages.Spanish, "Se eliminó al barco"));

            AddText(AddTranslation("LoadingCompetitionGroups.."),
                new Tuple<string, string>(Languages.English, "Boat was removed"),
                new Tuple<string, string>(Languages.Spanish, "Se eliminó al barco"));
            AddText(AddTranslation("NoGroupsAsddedYet"),
                new Tuple<string, string>(Languages.English, "Boat was removed"),
                new Tuple<string, string>(Languages.Spanish, "Se eliminó al barco"));
            AddText(AddTranslation("DeadlineFor"),
                new Tuple<string, string>(Languages.English, "Deadline for"),
                new Tuple<string, string>(Languages.Spanish, "Fecha límite para"));
            AddText(AddTranslation("LoadingTeamBoats.."),
                new Tuple<string, string>(Languages.English, "Loading team boats.."),
                new Tuple<string, string>(Languages.Spanish, "Cargando barcos del equipo.."));
            AddText(AddTranslation("Boat"),
                new Tuple<string, string>(Languages.English, "Boat"),
                new Tuple<string, string>(Languages.Spanish, "Barco"));
            AddText(AddTranslation("SailNumber"),
                new Tuple<string, string>(Languages.English, "Sail number"),
                new Tuple<string, string>(Languages.Spanish, "Número de vela"));
            AddText(AddTranslation("NoBoatsAssignedYet"),
                new Tuple<string, string>(Languages.English, "No boats assigned yet"),
                new Tuple<string, string>(Languages.Spanish, "Aún no se asignó ningún barco"));
            AddText(AddTranslation("Assign"),
                new Tuple<string, string>(Languages.English, "Assign"),
                new Tuple<string, string>(Languages.Spanish, "Asignar"));

            AddText(AddTranslation("LanguageChanged"),
                new Tuple<string, string>(Languages.English, "Language was changed"),
                new Tuple<string, string>(Languages.Spanish, "Se cambió el idioma"));
            AddText(AddTranslation("DeleteCommitteeBoatReturn"),
                new Tuple<string, string>(Languages.English, "Delete committee boat return"),
                new Tuple<string, string>(Languages.Spanish, "Eliminar vuelta de lancha del comité"));
            AddText(AddTranslation("CommitteeBoatReturn"),
                new Tuple<string, string>(Languages.English, "Committee boat return"),
                new Tuple<string, string>(Languages.Spanish, "Vuelta de lancha del comité"));
            AddText(AddTranslation("CommitteeBoatReturns"),
                new Tuple<string, string>(Languages.English, "Committee boat returns"),
                new Tuple<string, string>(Languages.Spanish, "Vueltas de lancha del comité"));
            AddText(AddTranslation("RegisterCommitteeBoatReturn"),
                new Tuple<string, string>(Languages.English, "Register committee boat return"),
                new Tuple<string, string>(Languages.Spanish, "Registrar vuelta de lancha del comité"));
            AddText(AddTranslation("CommitteeBoatName"),
                new Tuple<string, string>(Languages.English, "Committee boat name"),
                new Tuple<string, string>(Languages.Spanish, "Nombre de lancha del comité"));
            AddText(AddTranslation("Register"),
                new Tuple<string, string>(Languages.English, "Register"),
                new Tuple<string, string>(Languages.Spanish, "Registrar"));
            AddText(AddTranslation("RegisterReturn"),
                new Tuple<string, string>(Languages.English, "Register return"),
                new Tuple<string, string>(Languages.Spanish, "Registrar vuelta"));
            AddText(AddTranslation("LoadingCommitteeBoatReturns.."),
                new Tuple<string, string>(Languages.English, "Loading committee boat returns.."),
                new Tuple<string, string>(Languages.Spanish, "Cargando vueltas de lancha del comité"));
            AddText(AddTranslation("Unreported"),
                new Tuple<string, string>(Languages.English, "Unreported"),
                new Tuple<string, string>(Languages.Spanish, "Sin informar"));
            AddText(AddTranslation("BoatName"),
                new Tuple<string, string>(Languages.English, "Boat name"),
                new Tuple<string, string>(Languages.Spanish, "Nombre de lancha"));
            AddText(AddTranslation("NoCommitteeBoatReturnsYet"),
                new Tuple<string, string>(Languages.English, "No committee boat returns yet"),
                new Tuple<string, string>(Languages.Spanish, "Aún sin vueltas de lancha del comité"));
            AddText(AddTranslation("ReturnTime"),
                new Tuple<string, string>(Languages.English, "Return time"),
                new Tuple<string, string>(Languages.Spanish, "Momento de la vuelta"));
            AddText(AddTranslation("CommitteeBoatReturnWasRemoved"),
                new Tuple<string, string>(Languages.English, "Committee boat return was removed"),
                new Tuple<string, string>(Languages.Spanish, "Vuelta de lancha de comité eliminada"));


            AddText(AddTranslation("DeleteNotification"),
                new Tuple<string, string>(Languages.English, "Delete notification"),
                new Tuple<string, string>(Languages.Spanish, "Eliminar notificación"));
            AddText(AddTranslation("NotificationWasDeleted"),
                new Tuple<string, string>(Languages.English, "Notification was deleted"),
                new Tuple<string, string>(Languages.Spanish, "Notificación elminada"));
            AddText(AddTranslation("Notifications"),
                new Tuple<string, string>(Languages.English, "Notifications"),
                new Tuple<string, string>(Languages.Spanish, "Notificaciones"));
            AddText(AddTranslation("LoadingNotifications.."),
                new Tuple<string, string>(Languages.English, "Loading notifications.."),
                new Tuple<string, string>(Languages.Spanish, "Cargando notificaciones.."));
            AddText(AddTranslation("NoNotificationsYet"),
                new Tuple<string, string>(Languages.English, "No notifications yet"),
                new Tuple<string, string>(Languages.Spanish, "Aún sin notificaciones"));
            AddText(AddTranslation("Title"),
                new Tuple<string, string>(Languages.English, "Title"),
                new Tuple<string, string>(Languages.Spanish, "Título"));
            AddText(AddTranslation("Message"),
                new Tuple<string, string>(Languages.English, "Message"),
                new Tuple<string, string>(Languages.Spanish, "Mensaje"));
            AddText(AddTranslation("Author"),
                new Tuple<string, string>(Languages.English, "Author"),
                new Tuple<string, string>(Languages.Spanish, "Autor"));
            AddText(AddTranslation("CreationDate"),
                new Tuple<string, string>(Languages.English, "Creationd ate"),
                new Tuple<string, string>(Languages.Spanish, "Fecha de creación"));
            AddText(AddTranslation("CreateNotification"),
                new Tuple<string, string>(Languages.English, "Create notification"),
                new Tuple<string, string>(Languages.Spanish, "Crear notificación"));

            AddText(AddTranslation("IdIsRequired"),
                new Tuple<string, string>(Languages.English, "Id is a required field"),
                new Tuple<string, string>(Languages.Spanish, "Falta el campo identificador del registro"));
            AddText(AddTranslation("NameIsRequired"),
                new Tuple<string, string>(Languages.English, "Name is a required field"),
                new Tuple<string, string>(Languages.Spanish, "Se debe indicar un nombre"));
            AddText(AddTranslation("NameIsTooShort"),
                new Tuple<string, string>(Languages.English, "Name is too short"),
                new Tuple<string, string>(Languages.Spanish, "El nombre es demasiado corto"));
            AddText(AddTranslation("IdOrganizationIsRequired"),
                new Tuple<string, string>(Languages.English, "Club is a required field"),
                new Tuple<string, string>(Languages.Spanish, "Se debe indicar un club"));
            AddText(AddTranslation("IdChampionshipIsRequired"),
                new Tuple<string, string>(Languages.English, "Championship is a required field"),
                new Tuple<string, string>(Languages.Spanish, "Se debe indicar una campeonato"));
            AddText(AddTranslation("IdMemberIsRequired"),
                new Tuple<string, string>(Languages.English, "Member is a required field"),
                new Tuple<string, string>(Languages.Spanish, "Se debe indicar un integrante"));
            AddText(AddTranslation("IdRoleIsRequired"),
                new Tuple<string, string>(Languages.English, "Role is a required field"),
                new Tuple<string, string>(Languages.Spanish, "Se debe indicar un rol"));
            AddText(AddTranslation("IdBoatIsRequired"),
                new Tuple<string, string>(Languages.English, "You must select a boat"),
                new Tuple<string, string>(Languages.Spanish, "Se debe indicar un barco"));            
            AddText(AddTranslation("IdRaceClassIsRequired"),
                new Tuple<string, string>(Languages.English, "Race class is a required field"),
                new Tuple<string, string>(Languages.Spanish, "Se debe indicar una clase de carrera"));
            AddText(AddTranslation("RequiredField"),
                new Tuple<string, string>(Languages.English, "Required field"),
                new Tuple<string, string>(Languages.Spanish, "Campo requerido"));
            AddText(AddTranslation("CouldNotSaveFile"),
                new Tuple<string, string>(Languages.English, "File could not be saved"),
                new Tuple<string, string>(Languages.Spanish, "No se pudo guardar el archivo"));
            AddText(AddTranslation("DeleteFailed"),
                new Tuple<string, string>(Languages.English, "Delete failed"),
                new Tuple<string, string>(Languages.Spanish, "No se pudo eliminar"));
            AddText(AddTranslation("RecordCouldNotBeDeleted"),
                new Tuple<string, string>(Languages.English, "Record could not be deleted"),
                new Tuple<string, string>(Languages.Spanish, "No se pudo eliminar el registro"));
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

            AddText(AddTranslation("FirstnameIsRequired"),
                new Tuple<string, string>(Languages.English, "Firstname is required"),
                new Tuple<string, string>(Languages.Spanish, "El nombre es requerido"));
            AddText(AddTranslation("LastnameIsRequired"),
                new Tuple<string, string>(Languages.English, "Lastname is required"),
                new Tuple<string, string>(Languages.Spanish, "El apellido es requerido"));
            AddText(AddTranslation("EmailIsRequired"),
                new Tuple<string, string>(Languages.English, "Email is required"),
                new Tuple<string, string>(Languages.Spanish, "El email es requerido"));
            AddText(AddTranslation("PasswordIsRequired"),
                new Tuple<string, string>(Languages.English, "Password is required"),
                new Tuple<string, string>(Languages.Spanish, "La contraseña es requerida"));
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

            AddText(AddTranslation("Country"),
                new Tuple<string, string>(Languages.English, "Country"),
                new Tuple<string, string>(Languages.Spanish, "País"));
            AddText(AddTranslation("City"),
                new Tuple<string, string>(Languages.English, "City"),
                new Tuple<string, string>(Languages.Spanish, "Ciudad"));

            AddText(AddTranslation("Home"),
                new Tuple<string, string>(Languages.English, "Home"),
                new Tuple<string, string>(Languages.Spanish, "Inicio"));
            AddText(AddTranslation("Organization"),
                new Tuple<string, string>(Languages.English, "Club"),
                new Tuple<string, string>(Languages.Spanish, "Club"));
            AddText(AddTranslation("Organizations"),
                new Tuple<string, string>(Languages.English, "Clubs"),
                new Tuple<string, string>(Languages.Spanish, "Clubes"));
            AddText(AddTranslation("OrganizationDetails"),
                new Tuple<string, string>(Languages.English, "Organization details"),
                new Tuple<string, string>(Languages.Spanish, "Detalle del club"));
            AddText(AddTranslation("Team"),
                new Tuple<string, string>(Languages.English, "Team"),
                new Tuple<string, string>(Languages.Spanish, "Equipo"));
            AddText(AddTranslation("Teams"),
                new Tuple<string, string>(Languages.English, "Teams"),
                new Tuple<string, string>(Languages.Spanish, "Equipos"));
            AddText(AddTranslation("TeamDetails"),
                new Tuple<string, string>(Languages.English, "Team details"),
                new Tuple<string, string>(Languages.Spanish, "Detalle del equipo"));
            AddText(AddTranslation("Championship"),
                new Tuple<string, string>(Languages.English, "Championship"),
                new Tuple<string, string>(Languages.Spanish, "Campeonato"));
            AddText(AddTranslation("Championships"),
                new Tuple<string, string>(Languages.English, "Championships"),
                new Tuple<string, string>(Languages.Spanish, "Campeonatos"));
            AddText(AddTranslation("ChampionshipDetails"),
                new Tuple<string, string>(Languages.English, "Championship details"),
                new Tuple<string, string>(Languages.Spanish, "Detalle del campeonato"));

            AddText(AddTranslation("RegistrationTermDates"),
                new Tuple<string, string>(Languages.English, "Registration term dates"),
                new Tuple<string, string>(Languages.Spanish, "Plazo de fechas para registración"));
            AddText(AddTranslation("AccreditationTermDates"),
                new Tuple<string, string>(Languages.English, "Accreditation term dates"),
                new Tuple<string, string>(Languages.Spanish, "Plazo de fechas para acreditación"));
            AddText(AddTranslation("CompetitionTermDates"),
                new Tuple<string, string>(Languages.English, "Competition term dates"),
                new Tuple<string, string>(Languages.Spanish, "Plazo de fechas para competición"));


            AddText(AddTranslation("Class"),
                new Tuple<string, string>(Languages.English, "Class"),
                new Tuple<string, string>(Languages.Spanish, "Clase"));
            AddText(AddTranslation("Classes"),
                new Tuple<string, string>(Languages.English, "Classes"),
                new Tuple<string, string>(Languages.Spanish, "Clases"));

            AddText(AddTranslation("RaceCategory"),
                new Tuple<string, string>(Languages.English, "Category"),
                new Tuple<string, string>(Languages.Spanish, "Categoría"));
            AddText(AddTranslation("RaceClass"),
                new Tuple<string, string>(Languages.English, "Class"),
                new Tuple<string, string>(Languages.Spanish, "Clase"));
            AddText(AddTranslation("RaceClasses"),
                new Tuple<string, string>(Languages.English, "Classes"),
                new Tuple<string, string>(Languages.Spanish, "Clases"));
            AddText(AddTranslation("Race"),
                new Tuple<string, string>(Languages.English, "Race"),
                new Tuple<string, string>(Languages.Spanish, "Carrera"));
            AddText(AddTranslation("Races"),
                new Tuple<string, string>(Languages.English, "Races"),
                new Tuple<string, string>(Languages.Spanish, "Carreras"));

            AddText(AddTranslation("Group"),
                new Tuple<string, string>(Languages.English, "Group"),
                new Tuple<string, string>(Languages.Spanish, "Grupo"));
            AddText(AddTranslation("Groups"),
                new Tuple<string, string>(Languages.English, "Groups"),
                new Tuple<string, string>(Languages.Spanish, "Grupos"));
            AddText(AddTranslation("Registration"),
                new Tuple<string, string>(Languages.English, "Registration"),
                new Tuple<string, string>(Languages.Spanish, "Registración"));
            AddText(AddTranslation("Accreditation"),
                new Tuple<string, string>(Languages.English, "Accreditation"),
                new Tuple<string, string>(Languages.Spanish, "Acreditación"));
            AddText(AddTranslation("Competition"),
                new Tuple<string, string>(Languages.English, "Competition"),
                new Tuple<string, string>(Languages.Spanish, "Competición"));


            AddText(AddTranslation("Person"),
                new Tuple<string, string>(Languages.English, "Person"),
                new Tuple<string, string>(Languages.Spanish, "Persona"));
            AddText(AddTranslation("Persons"),
                new Tuple<string, string>(Languages.English, "Persons"),
                new Tuple<string, string>(Languages.Spanish, "Personas"));
            AddText(AddTranslation("PersonDetails"),
                new Tuple<string, string>(Languages.English, "Person details"),
                new Tuple<string, string>(Languages.Spanish, "Detalle de la persona"));
            AddText(AddTranslation("Firstname"),
                new Tuple<string, string>(Languages.English, "First name"),
                new Tuple<string, string>(Languages.Spanish, "Nombre"));
            AddText(AddTranslation("Lastname"),
                new Tuple<string, string>(Languages.English, "Last name"),
                new Tuple<string, string>(Languages.Spanish, "Apellido"));
            AddText(AddTranslation("BirthDate"),
                new Tuple<string, string>(Languages.English, "Birth date"),
                new Tuple<string, string>(Languages.Spanish, "Fecha de nacimiento"));
            AddText(AddTranslation("ContactPhone"),
                new Tuple<string, string>(Languages.English, "Contact phone"),
                new Tuple<string, string>(Languages.Spanish, "Teléfono de contacto"));
            AddText(AddTranslation("BloodType"),
                new Tuple<string, string>(Languages.English, "Blood type"),
                new Tuple<string, string>(Languages.Spanish, "Tipo de sangre"));
            AddText(AddTranslation("MedicalInsurance"),
                new Tuple<string, string>(Languages.English, "Medical insurance"),
                new Tuple<string, string>(Languages.Spanish, "Obra social"));
            AddText(AddTranslation("Email"),
                new Tuple<string, string>(Languages.English, "E-mail"),
                new Tuple<string, string>(Languages.Spanish, "E-mail"));

            AddText(AddTranslation("IamChampsionshipManager"),
                new Tuple<string, string>(Languages.English, "I'm a championship manager"),
                new Tuple<string, string>(Languages.Spanish, "Soy organizador de campeonatos"));

            AddText(AddTranslation("NoResultsFoundFor"),
                new Tuple<string, string>(Languages.English, "No results found for"),
                new Tuple<string, string>(Languages.Spanish, "No se encontraron resultados para"));
            AddText(AddTranslation("TypeFirstnameOrLastname"),
                new Tuple<string, string>(Languages.English, "Type firstname or lastname"),
                new Tuple<string, string>(Languages.Spanish, "Escriba nombre o apellido"));

            AddText(AddTranslation("Role"),
                new Tuple<string, string>(Languages.English, "Role"),
                new Tuple<string, string>(Languages.Spanish, "Rol"));
            AddText(AddTranslation("Status"),
                new Tuple<string, string>(Languages.English, "Status"),
                new Tuple<string, string>(Languages.Spanish, "Estado"));

            AddText(AddTranslation("ClickHere"),
                new Tuple<string, string>(Languages.English, "Click here"),
                new Tuple<string, string>(Languages.Spanish, "Haga click aquí"));
            AddText(AddTranslation("..OrJustWait"),
                new Tuple<string, string>(Languages.English, ".. or just wait"),
                new Tuple<string, string>(Languages.Spanish, ".. o espere"));

            AddText(AddTranslation("CreateNewAccount"),
                new Tuple<string, string>(Languages.English, "Create new account"),
                new Tuple<string, string>(Languages.Spanish, "Crear nueva cuenta"));
            AddText(AddTranslation("CreateNewOrganization"),
                new Tuple<string, string>(Languages.English, "Create new club"),
                new Tuple<string, string>(Languages.Spanish, "Crear nuevo club"));
            AddText(AddTranslation("CreateNewChampionship"),
                new Tuple<string, string>(Languages.English, "Create new championship"),
                new Tuple<string, string>(Languages.Spanish, "Crear nuevo campeonato"));
            AddText(AddTranslation("CreateNewTeam"),
                new Tuple<string, string>(Languages.English, "Create new team"),
                new Tuple<string, string>(Languages.Spanish, "Crear nuevo equipo"));
            AddText(AddTranslation("WelcomeToRaceBoard"),
                new Tuple<string, string>(Languages.English, "Welcome to RaceBoard"),
                new Tuple<string, string>(Languages.Spanish, "Bienvenido a RaceBoard"));
            AddText(AddTranslation("LandingWelcomeMessage"),
                new Tuple<string, string>(Languages.English, "Access championship results, get regattas latest status, and get in touch with the committee"),
                new Tuple<string, string>(Languages.Spanish, "Accede a los resultados de los campeonatos, informate sobre el estado de las regatas y comunicate con la comisión"));
            AddText(AddTranslation("RememberMe"),
                new Tuple<string, string>(Languages.English, "Remember me"),
                new Tuple<string, string>(Languages.Spanish, "Recuérdame"));
            AddText(AddTranslation("Enter"),
                new Tuple<string, string>(Languages.English, "Enter"),
                new Tuple<string, string>(Languages.Spanish, "Entrar"));
            AddText(AddTranslation("OrStartSessionWith"),
                new Tuple<string, string>(Languages.English, "Or start session with"),
                new Tuple<string, string>(Languages.Spanish, "O iniciar sesión con"));
            AddText(AddTranslation("DontHaveAnAccountYet?"),
                new Tuple<string, string>(Languages.English, "Don't have an account yet?"),
                new Tuple<string, string>(Languages.Spanish, "¿Aún no tienes una cuenta?"));
            AddText(AddTranslation("SuccessulLogin"),
                new Tuple<string, string>(Languages.English, "Successul login"),
                new Tuple<string, string>(Languages.Spanish, "Login exitoso"));
            AddText(AddTranslation("Welcome"),
                new Tuple<string, string>(Languages.English, "Welcome"),
                new Tuple<string, string>(Languages.Spanish, "Bienvenido"));

            AddText(AddTranslation("Welcome"),
                new Tuple<string, string>(Languages.English, "Welcome"),
                new Tuple<string, string>(Languages.Spanish, "Bienvenido"));
            AddText(AddTranslation("LogIn"),
                new Tuple<string, string>(Languages.English, "Login"),
                new Tuple<string, string>(Languages.Spanish, "Entrar"));
            AddText(AddTranslation("LogOut"),
                new Tuple<string, string>(Languages.English, "Logout"),
                new Tuple<string, string>(Languages.Spanish, "Cerrar sesión"));
            AddText(AddTranslation("YouHaveSuccessfullyLogout"),
                new Tuple<string, string>(Languages.English, "You have successfully logout"),
                new Tuple<string, string>(Languages.Spanish, "Haz cerrado la sesión exitosamente"));
            AddText(AddTranslation("Username"),
                new Tuple<string, string>(Languages.English, "Username"),
                new Tuple<string, string>(Languages.Spanish, "Nombre de usuario"));
            AddText(AddTranslation("Password"),
                new Tuple<string, string>(Languages.English, "Password"),
                new Tuple<string, string>(Languages.Spanish, "Contraseña"));
            AddText(AddTranslation("ForgotData?"),
                new Tuple<string, string>(Languages.English, "Forgot data?"),
                new Tuple<string, string>(Languages.Spanish, "Olvidó su contraseña?"));            
            AddText(AddTranslation("PrivacyPolicy"),
                new Tuple<string, string>(Languages.English, "Privacy policy"),
                new Tuple<string, string>(Languages.Spanish, "Acuerdo de privacidad"));
            AddText(AddTranslation("DisclosureStatement"),
                new Tuple<string, string>(Languages.English, "Disclosure statement"),
                new Tuple<string, string>(Languages.Spanish, "Política de divulgación"));
            AddText(AddTranslation("ResetPassword"),
                new Tuple<string, string>(Languages.English, "Reset password"),
                new Tuple<string, string>(Languages.Spanish, "Recuperar contraseña"));
            AddText(AddTranslation("ContentTextResetPassword"),
                new Tuple<string, string>(Languages.English, "Fill in the data to receive an e-mail with instructions to reset your password"),
                new Tuple<string, string>(Languages.Spanish, "Complete los datos para recibir un correo con instrucciones para reestablecer su contraseña"));
            AddText(AddTranslation("UserOrEmail"),
                new Tuple<string, string>(Languages.English, "Username or e-mail"),
                new Tuple<string, string>(Languages.Spanish, "Nombre de usuario o e-mail"));
            AddText(AddTranslation("Continue"),
                new Tuple<string, string>(Languages.English, "Continue"),
                new Tuple<string, string>(Languages.Spanish, "Continuar"));
            AddText(AddTranslation("Choose"),
                new Tuple<string, string>(Languages.English, "Choose"),
                new Tuple<string, string>(Languages.Spanish, "Seleccionar"));
            AddText(AddTranslation("CreatedSuccessfully"),
                new Tuple<string, string>(Languages.Spanish, "Creado exitosamente"));
            AddText(AddTranslation("ModifiedSuccessfully"),
                new Tuple<string, string>(Languages.Spanish, "Modificado exitosamente"));
            AddText(AddTranslation("DeletedSuccessfully"),
                new Tuple<string, string>(Languages.Spanish, "Eliminado exitosamente"));
            AddText(AddTranslation("SavedSuccessfully"),
                new Tuple<string, string>(Languages.English, "Data saved"),
                new Tuple<string, string>(Languages.Spanish, "Datos guardados"));
            AddText(AddTranslation("StudioManagement"),
                new Tuple<string, string>(Languages.Spanish, "Administración del estudio"));

            AddText(AddTranslation("Save"),
                new Tuple<string, string>(Languages.English, "Save"),
                new Tuple<string, string>(Languages.Spanish, "Guardar"));
            AddText(AddTranslation("Create"),
                new Tuple<string, string>(Languages.English, "Create"),
                new Tuple<string, string>(Languages.Spanish, "Crear"));
            AddText(AddTranslation("Add"),
                new Tuple<string, string>(Languages.English, "Add"),
                new Tuple<string, string>(Languages.Spanish, "Agregar"));
            AddText(AddTranslation("Edit"),
                new Tuple<string, string>(Languages.English, "Edit"),
                new Tuple<string, string>(Languages.Spanish, "Modificar"));
            AddText(AddTranslation("Remove"),
                new Tuple<string, string>(Languages.English, "Remove"),
                new Tuple<string, string>(Languages.Spanish, "Eliminar"));
            AddText(AddTranslation("Delete"),
                new Tuple<string, string>(Languages.English, "Delete"),
                new Tuple<string, string>(Languages.Spanish, "Eliminar"));
            AddText(AddTranslation("Confirm"),
                new Tuple<string, string>(Languages.English, "Confirm"),
                new Tuple<string, string>(Languages.Spanish, "Confirmar"));
            AddText(AddTranslation("Accept"),
                new Tuple<string, string>(Languages.English, "Accept"),
                new Tuple<string, string>(Languages.Spanish, "Aceptar"));
            AddText(AddTranslation("Cancel"),
                new Tuple<string, string>(Languages.English, "Cancel"),
                new Tuple<string, string>(Languages.Spanish, "Cancelar"));
            AddText(AddTranslation("AreYouSure?"),
                new Tuple<string, string>(Languages.English, "Are you sure?"),
                new Tuple<string, string>(Languages.Spanish, "¿Estás seguro?"));

            AddText(AddTranslation("SearchBy"),
                new Tuple<string, string>(Languages.Spanish, "Buscar por"));
            AddText(AddTranslation("Id"),
                new Tuple<string, string>(Languages.Spanish, "Id"));
            AddText(AddTranslation("Name"),
                 new Tuple<string, string>(Languages.English, "Name"),
                new Tuple<string, string>(Languages.Spanish, "Nombre"));
            AddText(AddTranslation("Type"),
                new Tuple<string, string>(Languages.Spanish, "Tipo"));

            AddText(AddTranslation("SetTeamName"),
                new Tuple<string, string>(Languages.English, "Set team name"),
                new Tuple<string, string>(Languages.Spanish, "Indicar nombre de equipo"));
            AddText(AddTranslation("ChooseChampionship"),
                new Tuple<string, string>(Languages.English, "Choose championship"),
                new Tuple<string, string>(Languages.Spanish, "Escoger campeonato"));
            AddText(AddTranslation("ChooseRaceClass"),
                new Tuple<string, string>(Languages.English, "Choose race class"),
                new Tuple<string, string>(Languages.Spanish, "Escoger clase"));

            AddText(AddTranslation("File"),
                new Tuple<string, string>(Languages.English, "File"),
                new Tuple<string, string>(Languages.Spanish, "Archivo"));
            AddText(AddTranslation("Files"),
                new Tuple<string, string>(Languages.English, "Files"),
                new Tuple<string, string>(Languages.Spanish, "Archivos"));
            AddText(AddTranslation("LoadingFiles.."),
                new Tuple<string, string>(Languages.English, "LoadingFiles.."),
                new Tuple<string, string>(Languages.Spanish, "Cargando archivos.."));
            AddText(AddTranslation("LoadingBoats.."),
                new Tuple<string, string>(Languages.English, "LoadingBoats.."),
                new Tuple<string, string>(Languages.Spanish, "Cargando barcos.."));
            AddText(AddTranslation("DeleteTeam"),
                new Tuple<string, string>(Languages.English, "DeleteTeam"),
                new Tuple<string, string>(Languages.Spanish, "Eliminar equipo"));
            AddText(AddTranslation("TeamBoat"),
                new Tuple<string, string>(Languages.English, "TeamBoat"),
                new Tuple<string, string>(Languages.Spanish, "Barco del equipo"));
            AddText(AddTranslation("DeleteTeamBoat"),
                new Tuple<string, string>(Languages.English, "DeleteTeamBoat"),
                new Tuple<string, string>(Languages.Spanish, "Eliminar barco del equipo"));
            AddText(AddTranslation("Upload"),
                new Tuple<string, string>(Languages.English, "Upload"),
                new Tuple<string, string>(Languages.Spanish, "Subir"));
            AddText(AddTranslation("CreationDate"),
                new Tuple<string, string>(Languages.English, "CreationDate"),
                new Tuple<string, string>(Languages.Spanish, "Fecha de creación"));
            AddText(AddTranslation("CreateGroup"),
                new Tuple<string, string>(Languages.English, "CreateGroup"),
                new Tuple<string, string>(Languages.Spanish, "Crear grupo"));
            AddText(AddTranslation("EditGroup"),
                new Tuple<string, string>(Languages.English, "EditGroup"),
                new Tuple<string, string>(Languages.Spanish, "Modificar grupo"));
            AddText(AddTranslation("DeleteGroup"),
                new Tuple<string, string>(Languages.English, "DeleteGroup"),
                new Tuple<string, string>(Languages.Spanish, "Eliminar grupo"));
            AddText(AddTranslation("NoFilesUploadedYet"),
                new Tuple<string, string>(Languages.English, "NoFilesUploadedYet"),
                new Tuple<string, string>(Languages.Spanish, "Aún no se subieron archivos"));

            AddText(AddTranslation("GroupRaceClasses"),
                new Tuple<string, string>(Languages.English, "Group classes"),
                new Tuple<string, string>(Languages.Spanish, "Clases del grupo"));
            AddText(AddTranslation("UploadFile"),
                new Tuple<string, string>(Languages.English, "Upload file"),
                new Tuple<string, string>(Languages.Spanish, "Subir archivo"));
            AddText(AddTranslation("Image"),
                new Tuple<string, string>(Languages.English, "Image"),
                new Tuple<string, string>(Languages.Spanish, "Imagen"));
            AddText(AddTranslation("FileType"),
                new Tuple<string, string>(Languages.English, "File type"),
                new Tuple<string, string>(Languages.Spanish, "Tipo de archivo"));
            AddText(AddTranslation("LoadingFileTypes.."),
                new Tuple<string, string>(Languages.English, "Loading file types.."),
                new Tuple<string, string>(Languages.Spanish, "Cargando tipos de archivo.."));
            AddText(AddTranslation("Flag"),
                new Tuple<string, string>(Languages.English, "Flag"),
                new Tuple<string, string>(Languages.Spanish, "Bandera"));
            AddText(AddTranslation("Flags"),
                new Tuple<string, string>(Languages.English, "Flags"),
                new Tuple<string, string>(Languages.Spanish, "Banderas"));
            AddText(AddTranslation("RaisedAt"),
                new Tuple<string, string>(Languages.English, "Raised at"),
                new Tuple<string, string>(Languages.Spanish, "Izada a las "));
            AddText(AddTranslation("LoweredAt"),
                new Tuple<string, string>(Languages.English, "Lowered at"),
                new Tuple<string, string>(Languages.Spanish, "Bajada a las"));
            AddText(AddTranslation("Refresh"),
                new Tuple<string, string>(Languages.English, "Refresh"),
                new Tuple<string, string>(Languages.Spanish, "Actualizar"));
            AddText(AddTranslation("FlagPole"),
                new Tuple<string, string>(Languages.English, "Flag pole"),
                new Tuple<string, string>(Languages.Spanish, "Mástil"));
            AddText(AddTranslation("LoadingFlagPole.."),
                new Tuple<string, string>(Languages.English, "Loading flag pole.."),
                new Tuple<string, string>(Languages.Spanish, "Cargado el mástil.."));
            AddText(AddTranslation("LoadingFlags.."),
                new Tuple<string, string>(Languages.English, "Loading flags.."),
                new Tuple<string, string>(Languages.Spanish, "Cargando banderas.."));
            AddText(AddTranslation("Raise"),
                new Tuple<string, string>(Languages.English, "Raise"),
                new Tuple<string, string>(Languages.Spanish, "Izar"));
            AddText(AddTranslation("RaiseFlag"),
                new Tuple<string, string>(Languages.English, "Raise flag"),
                new Tuple<string, string>(Languages.Spanish, "Izar bandera"));
            AddText(AddTranslation("RaiseFlags"),
                new Tuple<string, string>(Languages.English, "Raise flags"),
                new Tuple<string, string>(Languages.Spanish, "Izar banderas"));
            AddText(AddTranslation("Lower"),
                new Tuple<string, string>(Languages.English, "Lower"),
                new Tuple<string, string>(Languages.Spanish, "Bajar"));
            AddText(AddTranslation("LowerFlag"),
                new Tuple<string, string>(Languages.English, "Lower flag"),
                new Tuple<string, string>(Languages.Spanish, "Bajar bandera"));
            AddText(AddTranslation("LowerFlags"),
                new Tuple<string, string>(Languages.English, "Lower flags"),
                new Tuple<string, string>(Languages.Spanish, "Bajar banderas"));
            AddText(AddTranslation("HasNoFlagsRaisedYet"),
                new Tuple<string, string>(Languages.English, "NoFlagsRaisedYet"),
                new Tuple<string, string>(Languages.Spanish, "Aún no se izaron banderas"));
            AddText(AddTranslation("LastUpdateAt"),
                new Tuple<string, string>(Languages.English, "Last update at"),
                new Tuple<string, string>(Languages.Spanish, "Última actualización a las"));
            AddText(AddTranslation("Hours"),
                new Tuple<string, string>(Languages.English, "Hours"),
                new Tuple<string, string>(Languages.Spanish, "Horas"));
            AddText(AddTranslation("Minutes"),
                new Tuple<string, string>(Languages.English, "Minutes"),
                new Tuple<string, string>(Languages.Spanish, "Minutos"));
            AddText(AddTranslation("Seconds"),
                new Tuple<string, string>(Languages.English, "Seconds"),
                new Tuple<string, string>(Languages.Spanish, "Segundos"));
            AddText(AddTranslation("WithoutLoweringSchedule"),
                new Tuple<string, string>(Languages.English, "WithoutLoweringSchedule"),
                new Tuple<string, string>(Languages.Spanish, "Sin horario de arriado"));

            AddText(AddTranslation("YouDoNotHaveRightsToAccessThisContent"),
                new Tuple<string, string>(Languages.English, "You do not have rights to access this content"),
                new Tuple<string, string>(Languages.Spanish, "No tienes permiso para acceder a este contenido")); 

            AddText(AddTranslation("NewFileHasBeenUploaded"),
                new Tuple<string, string>(Languages.English, "A new file has been uploaded to championship"),
                new Tuple<string, string>(Languages.Spanish, "Se ha subido un nuevo archivo al campeonato"));

            AddText(AddTranslation("NewFlagsHoisted"),
                new Tuple<string, string>(Languages.English, "New flag(s) have been hoisted"),
                new Tuple<string, string>(Languages.Spanish, "Se izaron nueva/s bandera(s)"));

            AddText(AddTranslation("Administrator"),
                new Tuple<string, string>(Languages.English, "Administrator"),
                new Tuple<string, string>(Languages.Spanish, "Administrador"));
            AddText(AddTranslation("Manager"),
                new Tuple<string, string>(Languages.English, "Manager"),
                new Tuple<string, string>(Languages.Spanish, "Organizador"));
            AddText(AddTranslation("Auxiliary"),
                new Tuple<string, string>(Languages.English, "Auxiliary"),
                new Tuple<string, string>(Languages.Spanish, "Auxiliar"));
            AddText(AddTranslation("Jury"),
                new Tuple<string, string>(Languages.English, "Jury"),
                new Tuple<string, string>(Languages.Spanish, "Jurado"));
            AddText(AddTranslation("Competitor"),
                new Tuple<string, string>(Languages.English, "Competitor"),
                new Tuple<string, string>(Languages.Spanish, "Competidor"));

            AddText(AddTranslation("Leader"),
                new Tuple<string, string>(Languages.English, "Leader"),
                new Tuple<string, string>(Languages.Spanish, "Líder"));
            AddText(AddTranslation("Helm"),
                new Tuple<string, string>(Languages.English, "Helm"),
                new Tuple<string, string>(Languages.Spanish, "Timonel"));
            AddText(AddTranslation("Crew"),
                new Tuple<string, string>(Languages.English, "Crew"),
                new Tuple<string, string>(Languages.Spanish, "Tripulante"));
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
