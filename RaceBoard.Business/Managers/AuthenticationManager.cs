using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Exceptions;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Data;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Business.Managers
{
    public class AuthenticationManager : AbstractManager, IAuthenticationManager
    {
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly ICryptographyHelper _cryptographyHelper;

        #region Constructors

        public AuthenticationManager
            (
                IAuthenticationRepository authenticationRepository,
                IRequestContextManager requestContextManager,
                ICryptographyHelper cryptographyHelper,
                ITranslator translator
            ) 
            : base(requestContextManager, translator)
        {
            _authenticationRepository = authenticationRepository;
            _cryptographyHelper = cryptographyHelper;
        }

        #endregion

        #region IAuthenticationManager implementation

        public void Login(UserLogin userLogin, ITransactionalContext? context = null)
        {
            UserPassword userPassword = _authenticationRepository.Login(userLogin, context);
            if (userPassword == null)
                throw new FunctionalException(Common.Enums.ErrorType.NotFound, Translate("UserWasNotFound"));

            bool isValid = ValidateUserPassword(userLogin.Password, userPassword.Password);
            if (!isValid)
                throw new FunctionalException(Common.Enums.ErrorType.ValidationError, Translate("InvalidCredentials"));
        }

        #endregion

        private bool ValidateUserPassword(string plainTextPassword, string hashedPassword)
        {
            return _cryptographyHelper.VerifyHash(hashedPassword, plainTextPassword);
        }
    }
}
