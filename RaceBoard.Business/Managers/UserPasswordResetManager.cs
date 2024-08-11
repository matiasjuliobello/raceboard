using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Business.Validators.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Common.Exceptions;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Data;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;
using Microsoft.Extensions.Configuration;

namespace RaceBoard.Business.Managers
{
    public class UserPasswordResetManager : AbstractManager, IUserPasswordResetManager
    {
        private readonly int _passwordResetTokenLength;
        private readonly int _passwordResetTokenLifetime;

        private readonly IUserManager _userManager;
        private readonly IUserPasswordResetRepository _userPasswordResetRepository;
        private readonly ICustomValidator<UserPasswordReset> _userPasswordResetValidator;
        private readonly ICryptographyHelper _cryptographyHelper;
        private readonly IStringHelper _stringHelper;
        private readonly IDateTimeHelper _dateTimeHelper;

        #region Constructors

        public UserPasswordResetManager
            (
                IUserManager userManager,
                IUserPasswordResetRepository userPasswordResetRepository,
                ICustomValidator<UserPasswordReset> userPasswordResetValidator,
                ICryptographyHelper cryptographyHelper,
                IStringHelper stringHelper,
                IDateTimeHelper dateTimeHelper,
                ITranslator translator,
                IConfiguration configuration
            )
            : base(translator)
        {
            _userManager = userManager;
            _cryptographyHelper = cryptographyHelper;
            _stringHelper = stringHelper;
            _dateTimeHelper = dateTimeHelper;
            _userPasswordResetRepository = userPasswordResetRepository;
            _userPasswordResetValidator = userPasswordResetValidator;

            _passwordResetTokenLength = Convert.ToInt32(configuration["PasswordResetToken_Length"]);
            _passwordResetTokenLifetime = Convert.ToInt32(configuration["PasswordResetToken_Lifetime"]);
        }

        #endregion

        #region IUserPasswordResetManager implementation

        public void Create(string userEmailAddress, ITransactionalContext? context = null)
        {
            var user = _userManager.GetByEmailAddress(userEmailAddress);
            if (user == null)
                throw new FunctionalException(ErrorType.NotFound, "No user found with given e-mail address.");

            string randomString = _stringHelper.GenerateRandomString(_passwordResetTokenLength);
            string randomStringHash = _cryptographyHelper.ComputeHash(randomString);

            var currentDateTime = _dateTimeHelper.GetCurrentTimestamp();

            var userPasswordReset = new UserPasswordReset()
            {
                User = new User() { Id = user.Id },
                Token = randomStringHash,
                RequestDate = currentDateTime,
                ExpirationDate = currentDateTime.AddMinutes(_passwordResetTokenLifetime),
                IsActive = true
            };

            if (!_userPasswordResetValidator.IsValid(userPasswordReset, Scenario.Create))
            {
                throw new FunctionalException(ErrorType.ValidationError, _userPasswordResetValidator.Errors);
            }

            _userPasswordResetRepository.Create(userPasswordReset);
        }

        public void Update(string token, string password, ITransactionalContext? context = null)
        {
            var userPasswordReset = _userPasswordResetRepository.GetByToken(token, isUsed: false, isActive: true);
            if (userPasswordReset == null)
            {
                throw new FunctionalException(ErrorType.Unauthorized, "No active Password Reset Token was found.");
            }

            var currentDateTime = _dateTimeHelper.GetCurrentTimestamp();

            if (userPasswordReset.ExpirationDate < currentDateTime)
            {
                throw new FunctionalException(ErrorType.Unauthorized, "Password Reset Token is expired.");
            }

            var transaction = _userPasswordResetRepository.GetTransactionalContext();

            try
            {
                userPasswordReset.UseDate = currentDateTime;
                userPasswordReset.IsUsed = true;
                userPasswordReset.IsActive = false;

                if (!_userPasswordResetValidator.IsValid(userPasswordReset, Scenario.Update))
                {
                    throw new FunctionalException(ErrorType.ValidationError, _userPasswordResetValidator.Errors);
                }

                _userPasswordResetRepository.Update(userPasswordReset, transaction);

                _userManager.SavePassword(userPasswordReset.User.Id, password, transaction);

                transaction.Confirm();
            }
            catch (FunctionalException)
            {
                transaction.Cancel();

                throw;
            }
            catch (Exception)
            {
                transaction.Cancel();

                throw;
            }
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
