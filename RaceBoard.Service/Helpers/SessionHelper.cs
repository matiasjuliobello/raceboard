using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Service.Helpers.Interfaces;

namespace RaceBoard.Service.Helpers
{
    public class SessionHelper : ISessionHelper
    {
        private readonly IUserManager _userManager;
        private readonly IUserSettingsManager _userSettingsManager;
        private readonly ICacheHelper _cacheHelper;

        private static class Keys
        {
            public static string User(string username)
            {
                return $"User_{username}";
            }

            public static string UserSettings(string username)
            {
                return $"UserSettings_{username}";
            }
        }

        public SessionHelper
            (
                ICacheHelper cacheHelper,
                IUserManager userManager,
                IUserSettingsManager userSettingsManager
            )
        {
            _cacheHelper = cacheHelper;
            _userManager = userManager;
            _userSettingsManager = userSettingsManager;
        }

        public User GetUser(string username)
        {
            string key = Keys.User(username);

            User user = _cacheHelper.Get<User>(key);
            if (user == null)
            {
                SetUser(username);

                user = _cacheHelper.Get<User>(key);
            }

            return user;
        }
        public void SetUser(string username)
        {
            string key = Keys.User(username);

            User user = _userManager.GetByUsername(username);

            this.ProtectUserData(user);

            _cacheHelper.Set(key, user);
        }

        public UserSettings GetUserSettings(string username)
        {
            string key = Keys.UserSettings(username);

            UserSettings userSettings = _cacheHelper.Get<UserSettings>(key);

            if (userSettings == null)
            {
                SetUserSettings(username);

                userSettings = _cacheHelper.Get<UserSettings>(key);
            }

            return userSettings;
        }

        public void SetUserSettings(string username)
        {
            string key = Keys.UserSettings(username);

            UserSettings userSettings = _userSettingsManager.Get(username);

            _cacheHelper.Set(key, userSettings);
        }

        #region Private Methods

        private void ProtectUserData(User user)
        {
            user.Password = string.Empty; // to avoid any potential risk of storing sensitive information on cache
        }

        #endregion
    }
}
