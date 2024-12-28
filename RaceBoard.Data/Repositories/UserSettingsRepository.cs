using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using TimeZone = RaceBoard.Domain.TimeZone;
using RaceBoard.Data.Repositories.Base.Abstract;

namespace RaceBoard.Data.Repositories
{
    public class UserSettingsRepository : AbstractRepository, IUserSettingsRepository
    {
        #region Private Members

        #endregion

        #region Constructors

        public UserSettingsRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IUserSettingsRepository implementation

        public UserSettings GetById(int id, ITransactionalContext? context = null)
        {
            return this.Get(new UserSettingsSearchFilter() { Id = id }, context: context).FirstOrDefault();
        }

        public UserSettings GetByIdUser(int idUser, ITransactionalContext? context = null)
        {
            return this.Get(new UserSettingsSearchFilter() { IdsUser = new int[] { idUser } }, context: context).FirstOrDefault();
        }

        public List<UserSettings> GetByIdsUser(int[] idsUser, ITransactionalContext? context = null)
        {
            return this.Get(new UserSettingsSearchFilter() { IdsUser = idsUser }, context: context);
        }

        public UserSettings GetByUsername(string username, ITransactionalContext? context = null)
        {
            return this.Get(new UserSettingsSearchFilter() { Username = username }, context: context).FirstOrDefault();
        }

        public void Create(UserSettings userSettings, ITransactionalContext? context = null)
        {
            string sql = $@"INSERT INTO [UserSettings] 
                                ( IdUser, IdLanguage, IdTimeZone )
                            VALUES
                                ( @idUser, @idLanguage, @idTimeZone )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idUser", userSettings.User.Id);
            QueryBuilder.AddParameter("idLanguage", userSettings.Language.Id);
            QueryBuilder.AddParameter("idTimeZone", userSettings.TimeZone.Id);

            QueryBuilder.AddReturnLastInsertedId();

            userSettings.Id = base.Execute<int>(context);
        }

        public void Update(UserSettings userSettings, ITransactionalContext? context = null)
        {
            string sql = $@"UPDATE [UserSettings]
                            SET
                                IdLanguage = @idLanguage,
                                IdTimeZone = @idTimeZone";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddCondition("IdUser = @idUser");
            QueryBuilder.AddParameter("idUser", userSettings.User.Id);

            QueryBuilder.AddParameter("idLanguage", userSettings.Language.Id);
            QueryBuilder.AddParameter("idTimeZone", userSettings.TimeZone.Id);

            base.ExecuteAndGetRowsAffected(context);
        }

        public int Delete(int id, ITransactionalContext? context = null)
        {
            return base.Delete("[UserSettings]", id);
        }

        #endregion

        #region Private Methods

        private List<UserSettings> Get(UserSettingsSearchFilter userSettingsSearchFilter, ITransactionalContext? context = null)
        {
            string sql = $@"
                       SELECT 
                            [UserSettings].Id [Id],
                            [UserSettings].IdUser [Id],
                            [Language].Id [Id],
                            [Language].Name [Name],
                            [Language].Code [Code],
                            [TimeZone].Id [Id],
                            [TimeZone].Name [Name],
                            [TimeZone].Identifier [Identifier],
                            [TimeZone].Offset [Offset],
                            [DateFormat].Id [Id],
                            [DateFormat].Format [Format]
                        FROM UserSettings
                        LEFT JOIN [User] ON [User].Id = [UserSettings].IdUser
                        LEFT JOIN Language ON [Language].Id = [UserSettings].IdLanguage
                        LEFT JOIN TimeZone ON [TimeZone].Id = [UserSettings].IdTimeZone
                        LEFT JOIN DateFormat ON [DateFormat].Id = [UserSettings].IdDateFormat";

            QueryBuilder.AddCommand(sql);

            this.ProcessSearchFilter(userSettingsSearchFilter);

            List<UserSettings> userSettingsList = null;

            base.GetReader
                (
                    (x) =>
                    {
                        userSettingsList = x.Read<UserSettings, User, Language, TimeZone, DateFormat, UserSettings>
                        (
                            (userSettings, user, language, timeZone, dateFormat) =>
                            {
                                userSettings.User = user;
                                userSettings.Language = language;
                                userSettings.TimeZone = timeZone;
                                userSettings.DateFormat = dateFormat;

                                return userSettings;
                            },
                            splitOn: "Id, Id, Id, Id, Id"
                        ).ToList();
                    },
                    context
                );

            return userSettingsList;
        }

        private void ProcessSearchFilter(UserSettingsSearchFilter? searchFilter = null)
        {
            base.AddFilterCriteria(ConditionType.Equal, "UserSettings", "Id", "ids", searchFilter.Id);
            base.AddFilterCriteria(ConditionType.In, "UserSettings", "IdUser", "idUser", searchFilter.IdsUser);
            base.AddFilterCriteria(ConditionType.Equal, "[User]", "Username", "userName", searchFilter.Username);
        }

        #endregion
    }
}
