using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Business.Validators.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Common.Exceptions;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Data;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;
using Microsoft.Extensions.Configuration;
using Enums = RaceBoard.Common.Enums;
using RaceBoard.Business.Helpers.Interfaces;

namespace RaceBoard.Business.Managers
{
    public class UserManager : AbstractManager, IUserManager
    {
        private readonly int _passwordResetTokenLength;
        private readonly int _passwordResetTokenLifetime;

        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserSettingsRepository _userSettingsRepository;
        private readonly ICustomValidator<User> _userValidator;
        private readonly ICustomValidator<UserPassword> _userPasswordValidator;
        private readonly ICryptographyHelper _cryptographyHelper;
        private readonly INotificationHelper _notificationHelper;
        private readonly IStringHelper _stringHelper;

        #region Constructors

        public UserManager
            (
                IUserRepository userRepository,
                IUserRoleRepository userRoleRepository,
                IUserSettingsRepository userSettingsRepository,
                ICustomValidator<User> userValidator,
                ICustomValidator<UserPassword> userPasswordValidator,
                ICryptographyHelper cryptographyHelper,
                IStringHelper stringHelper,
                INotificationHelper notificationHelper,
                ITranslator translator,
                IConfiguration configuration
            //IRequestContextManager requestContextManager
            ) : base(null, translator)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _userSettingsRepository = userSettingsRepository;
            _userValidator = userValidator;
            _userPasswordValidator = userPasswordValidator;
            _cryptographyHelper = cryptographyHelper;
            _notificationHelper = notificationHelper;
            _stringHelper = stringHelper;

            _passwordResetTokenLength = Convert.ToInt32(configuration["PasswordResetToken_Length"]);
            _passwordResetTokenLifetime = Convert.ToInt32(configuration["PasswordResetToken_Lifetime"]);
        }

        #endregion

        #region IUserManager implementation

        public PaginatedResult<User> Get(UserSearchFilter userSearchFilter, PaginationFilter? paginationFilter = null, Sorting? sorting = null, ITransactionalContext? context = null)
        {
            return _userRepository.Get(userSearchFilter, paginationFilter, sorting, context);
        }

        public User GetById(int id, ITransactionalContext? context = null)
        {
            return _userRepository.GetById(id, context);
        }

        public User GetByUsername(string username, ITransactionalContext? context = null)
        {
            return _userRepository.GetByUsername(username, context);
        }

        public User GetByEmailAddress(string emailAddress, ITransactionalContext? context = null)
        {
            return _userRepository.GetByEmailAddress(emailAddress, context);
        }

        public void Create(User user, ITransactionalContext? context = null)
        {
            user.IsActive = true;

            if (context == null)
                context = _userRepository.GetTransactionalContext();

            _userValidator.SetTransactionalContext(context);

            if (!_userValidator.IsValid(user, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _userValidator.Errors);

            try
            {
                //var userPassword = new UserPassword()
                //{
                //    IdUser = 0,
                //    Password = user.Password
                //};
                //if (!_userPasswordValidator.IsValid(userPassword, Scenario.Update))
                //    throw new FunctionalException(ErrorType.ValidationError, _userPasswordValidator.Errors);

                //string randomString = _stringHelper.GenerateRandomString(_passwordResetTokenLength);
                //string randomStringHash = _cryptographyHelper.ComputeHash(randomString);
                user.Password = _cryptographyHelper.ComputeHash(user.Password);

                _userRepository.Create(user, context);

                var userRole = new UserRole()
                {
                    User = user,
                    Role = new Role()
                    {
                        Id = user.UserRole.Role.Id
                    }
                };
                _userRoleRepository.Create(userRole, context);

                // TODO: remove this hardcoding
                var userSettings = new UserSettings()
                {
                    User = new User() { Id = user.Id },
                    TimeZone = new Domain.TimeZone() { Id = 1 },
                    Language = new Domain.Language() { Id = (int)Enums.Language.Spanish },
                    DateFormat = new DateFormat() { Id = 3 }
                };
                _userSettingsRepository.Create(userSettings, context);

                _notificationHelper.SendNotification(Notification.Enums.NotificationType.User_Creation, user);

                context.Confirm();
            }
            catch (Exception)
            {
                if (context != null)
                    context.Cancel();

                throw;
            }
        }

        public void Update(User user, ITransactionalContext? context = null)
        {
            if (!_userValidator.IsValid(user, Scenario.Update))
                throw new FunctionalException(ErrorType.ValidationError, _userValidator.Errors);

            var userPassword = new UserPassword()
            {
                IdUser = user.Id,
                Password = user.Password
            };

            if (!_userPasswordValidator.IsValid(userPassword, Scenario.Update))
                throw new FunctionalException(ErrorType.ValidationError, _userValidator.Errors);

            _userRepository.Update(user);
        }

        public User Delete(int id, ITransactionalContext? context = null)
        {
            var user = this.GetById(id, context);

            int affectedRecords = _userRepository.Delete(id);
            if (affectedRecords == 0)
                throw new FunctionalException(ErrorType.ValidationError, base.Translate("DeleteFailed"));

            return user;
        }

        public void SavePassword(int idUser, string password, ITransactionalContext? context = null)
        {
            var userPassword = new UserPassword()
            {
                IdUser = idUser,
                Password = password
            };

            if (!_userPasswordValidator.IsValid(userPassword, Scenario.Update))
                throw new FunctionalException(ErrorType.ValidationError, _userValidator.Errors);

            userPassword.Password = _cryptographyHelper.ComputeHash(password);

            _userRepository.SavePassword(userPassword, context);
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
