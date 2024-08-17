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
