using Newtonsoft.Json.Linq;
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
            AddText(AddTranslation("RecordNotFound"),
                new Tuple<string, string>(Languages.English, "Record not found"),
                new Tuple<string, string>(Languages.Spanish, "No se encontró el registro"));
            AddText(AddTranslation("Oops!LooksLikeYouAreLost"),
                new Tuple<string, string>(Languages.English, "Oops! Looks like you're lost.."),
                new Tuple<string, string>(Languages.Spanish, "Oops! Parece que estás perdido.."));
            AddText(AddTranslation("TheDataYouAreLookingForWasNotFound"),
                new Tuple<string, string>(Languages.English, "The data you are looking for was not found"),
                new Tuple<string, string>(Languages.Spanish, "No se encontraron los datos que estás buscando"));

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


            AddText(AddTranslation("UserPreferencesNotFound"),
                new Tuple<string, string>(Languages.Spanish, "No se encontró las preferencias del usuario"));
            AddText(AddTranslation("DuplicateRecordExists"),
                new Tuple<string, string>(Languages.English, "Looks there's already a record with same data"),
                new Tuple<string, string>(Languages.Spanish, "Parece que ya existe un registro con los mismos datos"));
            AddText(AddTranslation("CannotDeleteContestantDueToExistingParticipation"),
                new Tuple<string, string>(Languages.English, "Could not delete member due to existing participations with team"),
                new Tuple<string, string>(Languages.Spanish, "No se puede remover al integrante porque ya tuvo participaciones con en el equipo"));
            AddText(AddTranslation("BoatAlreadyAssignedToAnotherTeam"),
                new Tuple<string, string>(Languages.English, "The boat is already assigned to another team in the same competition"),
                new Tuple<string, string>(Languages.Spanish, "El barco ya está asignado a otro equipo de la misma competición"));
            AddText(AddTranslation("TeamWasDeleted"),
                new Tuple<string, string>(Languages.English, "Team was removed deleted"),
                new Tuple<string, string>(Languages.Spanish, "Se eliminó el equipo"));
            AddText(AddTranslation("GroupWasRemovedFromTheCompetition"),
                new Tuple<string, string>(Languages.English, "Group was removed from competition"),
                new Tuple<string, string>(Languages.Spanish, "Se eliminó al grupo de la competición"));
            AddText(AddTranslation("BoatWasRemovedFromTheTeam"),
                new Tuple<string, string>(Languages.English, "Boat was removed from team"),
                new Tuple<string, string>(Languages.Spanish, "Se eliminó al barco del equipo"));
            AddText(AddTranslation("MemberWasRemovedFromTheTeam"),
                new Tuple<string, string>(Languages.English, "Member was removed from the team"),
                new Tuple<string, string>(Languages.Spanish, "Se eliminó al integrante del equipo"));
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
                new Tuple<string, string>(Languages.English, "Organization is a required field"),
                new Tuple<string, string>(Languages.Spanish, "Se debe indicar una organización"));
            AddText(AddTranslation("IdCompetitionIsRequired"),
                new Tuple<string, string>(Languages.English, "Competition is a required field"),
                new Tuple<string, string>(Languages.Spanish, "Se debe indicar una competición"));
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

            AddText(AddTranslation("Home"),
                new Tuple<string, string>(Languages.English, "Home"),
                new Tuple<string, string>(Languages.Spanish, "Inicio"));
            AddText(AddTranslation("Teams"),
                new Tuple<string, string>(Languages.English, "Teams"),
                new Tuple<string, string>(Languages.Spanish, "Equipos"));
            AddText(AddTranslation("Competitions"),
                new Tuple<string, string>(Languages.English, "Competitions"),
                new Tuple<string, string>(Languages.Spanish, "Campeonatos"));
            AddText(AddTranslation("Organizations"),
                new Tuple<string, string>(Languages.English, "Organizations"),
                new Tuple<string, string>(Languages.Spanish, "Organizaciones"));
            AddText(AddTranslation("Team"),
                new Tuple<string, string>(Languages.English, "Team"),
                new Tuple<string, string>(Languages.Spanish, "Equipo"));
            AddText(AddTranslation("Competition"),
                new Tuple<string, string>(Languages.English, "Competition"),
                new Tuple<string, string>(Languages.Spanish, "Campeonato"));
            AddText(AddTranslation("Organization"),
                new Tuple<string, string>(Languages.English, "Organization"),
                new Tuple<string, string>(Languages.Spanish, "Organización"));


            AddText(AddTranslation("RaceClass"),
                new Tuple<string, string>(Languages.English, "Class"),
                new Tuple<string, string>(Languages.Spanish, "Clase"));
            AddText(AddTranslation("RaceCategory"),
                new Tuple<string, string>(Languages.English, "Category"),
                new Tuple<string, string>(Languages.Spanish, "Categoría"));


            AddText(AddTranslation("CreateNewTeam"),
                new Tuple<string, string>(Languages.English, "Create new team"),
                new Tuple<string, string>(Languages.Spanish, "Crear nuevo equipo"));

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
            AddText(AddTranslation("Choose"),
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
                 new Tuple<string, string>(Languages.English, "Name"),
                new Tuple<string, string>(Languages.Spanish, "Nombre"));
            AddText(AddTranslation("Type"),
                new Tuple<string, string>(Languages.Spanish, "Tipo"));
            AddText(AddTranslation("Members"),
                new Tuple<string, string>(Languages.English, "Members"),
                new Tuple<string, string>(Languages.Spanish, "Integrantes"));

            AddText(AddTranslation("SetTeamName"),
                new Tuple<string, string>(Languages.English, "Set team name"),
                new Tuple<string, string>(Languages.Spanish, "Indicar nombre de equipo"));
            AddText(AddTranslation("ChooseCompetition"),
                new Tuple<string, string>(Languages.English, "Choose competition"),
                new Tuple<string, string>(Languages.Spanish, "Escoger competición"));
            AddText(AddTranslation("ChooseRaceClass"),
                new Tuple<string, string>(Languages.English, "Choose race class"),
                new Tuple<string, string>(Languages.Spanish, "Escoger clase"));
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
