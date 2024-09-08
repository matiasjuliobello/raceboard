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
                            ( IdUser, IdCulture, IdLanguage, IdTimeZone )
                            VALUES
                            (
                                @idUser,
                                @idCulture,
                                @idLanguage,
                                @idTimeZone
                            )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idUser", userSettings.User.Id);
            QueryBuilder.AddParameter("idCulture", userSettings.Culture.Id);
            QueryBuilder.AddParameter("idLanguage", userSettings.Language.Id);
            QueryBuilder.AddParameter("idTimeZone", userSettings.TimeZone.Id);

            QueryBuilder.AddReturnLastInsertedId();

            userSettings.Id = base.Execute<int>(context);
        }

        public void Update(UserSettings userSettings, ITransactionalContext? context = null)
        {
            string sql = $@"UPDATE [UserSettings]
                            SET
                                IdCulture = @idCulture,
                                IdLanguage = @idLanguage
                                IdTimeZone = @idTimeZone";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddCondition("Id = @id");
            QueryBuilder.AddParameter("id", userSettings.Id);
            QueryBuilder.AddParameter("idCulture", userSettings.Culture.Id);
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
                            [Culture].Id [Id], 
                            [Culture].Name [Name],
                            [Culture].Description [Description],                         
                            [Language].Id [Id],
                            [Language].Name [Name],
                            [TimeZone].Id [Id],
                            [TimeZone].Name [Name],
                            [TimeZone].Identifier [Identifier],
                            [TimeZone].Offset [Offset]
                        FROM UserSettings
                        LEFT JOIN [User] ON [User].Id = [UserSettings].IdUser
                        LEFT JOIN Culture ON [Culture].Id = [UserSettings].IdCulture
                        LEFT JOIN Language ON [Language].Id = [UserSettings].IdLanguage
                        LEFT JOIN TimeZone ON [TimeZone].Id = [UserSettings].IdTimeZone";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(userSettingsSearchFilter);

            List<UserSettings> userSettingsList = null;

            base.GetReader
                (
                    (x) =>
                    {
                        userSettingsList = x.Read<UserSettings, User, Culture, Language, TimeZone, UserSettings>
                        (
                            (userSettings, user, culture, language, timeZone) =>
                            {
                                userSettings.User = user;
                                userSettings.Culture = culture;
                                userSettings.Language = language;
                                userSettings.TimeZone = timeZone;

                                return userSettings;
                            },
                            splitOn: "Id, Id, Id, Id, Id"
                        ).ToList();
                    },
                    context
                );

            return userSettingsList;
        }

        private void ProcessSearchFilter(UserSettingsSearchFilter searchFilter)
        {
            base.AddFilterCriteria(ConditionType.Equal, "UserSettings", "Id", searchFilter.Id);
            base.AddFilterCriteria(ConditionType.In, "UserSettings", "IdUser", searchFilter.IdsUser);
            base.AddFilterCriteria(ConditionType.Equal, "User", "Username", searchFilter.Username);
        }

        #endregion
    }
}
