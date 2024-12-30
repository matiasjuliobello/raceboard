using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using RaceBoard.Data.Repositories.Base.Abstract;

namespace RaceBoard.Data.Repositories
{
    public class UserAccessRepository : AbstractRepository, IUserAccessRepository
    {
        #region Private Members

        #endregion

        #region Constructors

        public UserAccessRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IUserAccessRepository implementation

        public UserAccess Get(int idUser, int idChampionship, ITransactionalContext? context = null)
        {
            return this.GetUserAccess(idUser, idChampionship, context);
        }

        public void Create(UserAccess userAccess, ITransactionalContext? context = null)
        {
            string sql = $@"INSERT INTO [User_Championship]
                            ( 
                                IdUser, IdRole, IdChampionship
                            )
                            VALUES
                            (
                                @idUser,
                                @idRole
                                @idChampionship
                            )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idUser", userAccess.User.Id);
            QueryBuilder.AddParameter("idRole", (object)userAccess.Role.Id);
            QueryBuilder.AddParameter("idChampionship", userAccess.Championship.Id);

            QueryBuilder.AddReturnLastInsertedId();

            userAccess.Id = base.Execute<int>(context);
        }

        public int Delete(int id, ITransactionalContext? context = null)
        {
            return base.Delete("[User_Championship]", id);
        }

        #endregion

        #region Private Methods

        private UserAccess GetUserAccess(int idUser, int idChampionship, ITransactionalContext? context = null)
        {
            string sql = $@"
                           SELECT 
                                [User_Championship].Id [Id],
                                [Role].Id [Id],
                                [Role].Name [Name],
                                [Championship].Id [Id],
                                [Championship].Name [Name]
                            FROM [User_Championship] [User_Championship]
                            INNER JOIN [User] ON [User].Id = [User_Championship].IdUser
                            INNER JOIN [Role] ON [Role].Id = [User_Championship].IdRole
                            INNER JOIN [Championship] ON [Championship].Id = [User_Championship].IdChampionship";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(idUser, idChampionship);

            var userAccesses = new List<UserAccess>();

            base.GetReader
                (
                    (x) =>
                    {
                        userAccesses = x.Read<UserAccess, User, Role, Championship, UserAccess>
                        (
                            (userAccess, user, userRole, championship) =>
                            {
                                userAccess.User = user;
                                userAccess.Role = userRole;
                                userAccess.Championship = championship;

                                return userAccess;
                            },
                            splitOn: "Id, Id, Id"
                        ).ToList();
                    },
                    context
                );

            return userAccesses.FirstOrDefault();
        }

        private void ProcessSearchFilter(int idUser, int idChampionship)
        {
            base.AddFilterCriteria(ConditionType.Equal, "User_Championship", "IdUser", "idUser", idUser);
            base.AddFilterCriteria(ConditionType.Equal, "User_Championship", "IdChampionship", "idChampionship", idChampionship);
        }

        #endregion
    }
}
