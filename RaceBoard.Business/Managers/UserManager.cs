﻿using RaceBoard.Business.Managers.Abstract;
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

namespace RaceBoard.Business.Managers
{
    public class UserManager : AbstractManager, IUserManager
    {
        private readonly int _passwordResetTokenLength;
        private readonly int _passwordResetTokenLifetime;

        private readonly IUserRepository _userRepository;
        private readonly ICustomValidator<User> _userValidator;
        private readonly ICustomValidator<UserPassword> _userPasswordValidator;
        private readonly ICryptographyHelper _cryptographyHelper;
        private readonly IStringHelper _stringHelper;

        #region Constructors

        public UserManager
            (
                IUserRepository userRepository,
                ICustomValidator<User> userValidator,
                ICustomValidator<UserPassword> userPasswordValidator,
                ICryptographyHelper cryptographyHelper,
                IStringHelper stringHelper,
                ITranslator translator,
                IConfiguration configuration
            ) : base(translator)
        {
            _userRepository = userRepository;
            _userValidator = userValidator;
            _userPasswordValidator = userPasswordValidator;
            _cryptographyHelper = cryptographyHelper;
            _stringHelper = stringHelper;

            _passwordResetTokenLength = Convert.ToInt32(configuration["PasswordResetToken_Length"]);
            _passwordResetTokenLifetime = Convert.ToInt32(configuration["PasswordResetToken_Lifetime"]);
        }

        #endregion

        #region IUserManager implementation

        public PaginatedResult<User> Get(UserSearchFilter userSearchFilter, PaginationFilter paginationFilter, Sorting sorting, ITransactionalContext? context = null)
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
            string randomString = _stringHelper.GenerateRandomString(_passwordResetTokenLength);
            string randomStringHash = _cryptographyHelper.ComputeHash(randomString);
            
            user.Password = randomStringHash;
            user.IsActive = true;

            if (!_userValidator.IsValid(user, Scenario.Create))
                throw new FunctionalException(ErrorType.ValidationError, _userValidator.Errors);

            _userRepository.Create(user);
        }

        public void Update(User user, ITransactionalContext? context = null)
        {
            if (!_userValidator.IsValid(user, Scenario.Update))
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

            userPassword.Password = _cryptographyHelper.ComputeHash(password);

            _userRepository.SavePassword(userPassword, context);
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
