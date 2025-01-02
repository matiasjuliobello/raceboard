using RaceBoard.Business.Managers.Abstract;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Enums;
using RaceBoard.Common.Exceptions;
using RaceBoard.Data;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Business.Managers
{
    public class UserSettingsManager : AbstractManager, IUserSettingsManager
    {
        private readonly IUserSettingsRepository _userSettingsRepository;

        #region Constructors

        public UserSettingsManager
            (
                IUserSettingsRepository userSettingsRepository,
                IRequestContextManager requestContextManager,
                ITranslator translator
            ) : base(requestContextManager, translator)
        {
            _userSettingsRepository = userSettingsRepository;
        }

        #endregion

        #region IUserSettingsManager implementation

        public UserSettings Get(int idUser, ITransactionalContext? context = null)
        {
            return this.GetUserSettings(new UserSettingsSearchFilter() { IdsUser = new int[] { idUser } }, context: context).FirstOrDefault();
        }

        public List<UserSettings> Get(int[] idsUser, ITransactionalContext? context = null)
        {
            return this.GetUserSettings(new UserSettingsSearchFilter() { IdsUser = idsUser }, context: context);
        }

        public UserSettings Get(string username, ITransactionalContext? context = null)
        {
            return this.GetUserSettings(new UserSettingsSearchFilter() { Username = username }, context: context).FirstOrDefault();
        }

        public void Create(UserSettings userSettings, ITransactionalContext? context = null)
        {
            _userSettingsRepository.Create(userSettings, context);
        }

        public void Update(UserSettings userSettings, ITransactionalContext? context = null)
        {
            _userSettingsRepository.Update(userSettings, context: context);
        }

        public UserSettings Delete(int id, ITransactionalContext? context = null)
        {
            var userSettings = this.GetUserSettings(new UserSettingsSearchFilter() { Id = id }, context: context);

            if (userSettings == null || userSettings.Count == 0)
                throw new FunctionalException(ErrorType.NotFound, base.Translate("RecordNotFound"));

            int affectedRecords = _userSettingsRepository.Delete(id, context);
            if (affectedRecords == 0)
                throw new FunctionalException(ErrorType.ValidationError, base.Translate("DeleteFailed"));

            return userSettings.First();
        }

        #endregion

        #region Private Methods

        private List<UserSettings> GetUserSettings(UserSettingsSearchFilter userSettingsSearchFilter, ITransactionalContext? context = null)
        {
            var userSettings = new List<UserSettings>();

            if (userSettingsSearchFilter.Id != null)
            {
                userSettings = new List<UserSettings>()
                {
                    _userSettingsRepository.GetById(userSettingsSearchFilter.Id.Value, context)
                };
            }

            if (userSettingsSearchFilter.IdsUser != null)
            {
                userSettings = _userSettingsRepository.GetByIdsUser(userSettingsSearchFilter.IdsUser, context);
            }

            if (!string.IsNullOrEmpty(userSettingsSearchFilter.Username))
            {
                userSettings = new List<UserSettings>()
                {
                    _userSettingsRepository.GetByUsername(userSettingsSearchFilter.Username, context)
                };
            }

            return userSettings;
        }

        #endregion
    }
}