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

        public UserAccess Get(int idUser, int idCompetition, ITransactionalContext? context = null)
        {
            return this.GetUserAccess(idUser, idCompetition, context);
        }

        public void Create(UserAccess userAccess, ITransactionalContext? context = null)
        {
            string sql = $@"INSERT INTO [User_Competition]
                            ( 
                                IdUser, IdRole, IdCompetition
                            )
                            VALUES
                            (
                                @idUser,
                                @idRole
                                @idCompetition
                            )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idUser", userAccess.User.Id);
            QueryBuilder.AddParameter("idRole", (object)userAccess.Role.Id);
            QueryBuilder.AddParameter("idCompetition", userAccess.Competition.Id);

            QueryBuilder.AddReturnLastInsertedId();

            userAccess.Id = base.Execute<int>(context);
        }

        public int Delete(int id, ITransactionalContext? context = null)
        {
            return base.Delete("[User_Competition]", id);
        }

        #endregion

        #region Private Methods

        private UserAccess GetUserAccess(int idUser, int idCompetition, ITransactionalContext? context = null)
        {
            string sql = $@"
                           SELECT 
                                [User_Competition].Id [Id],
                                [Role].Id [Id],
                                [Role].Name [Name],
                                [Competition].Id [Id],
                                [Competition].Name [Name]
                            FROM [User_Competition] [User_Competition]
                            INNER JOIN [User] ON [User].Id = [User_Competition].IdUser
                            INNER JOIN [Role] ON [Role].Id = [User_Competition].IdRole
                            INNER JOIN [Competition] ON [Competition].Id = [User_Competition].IdCompetition";

            QueryBuilder.AddCommand(sql);

            ProcessSearchFilter(idUser, idCompetition);

            var userAccesses = new List<UserAccess>();

            base.GetReader
                (
                    (x) =>
                    {
                        userAccesses = x.Read<UserAccess, User, Role, Competition, UserAccess>
                        (
                            (userAccess, user, userRole, competition) =>
                            {
                                userAccess.User = user;
                                userAccess.Role = userRole;
                                userAccess.Competition = competition;

                                return userAccess;
                            },
                            splitOn: "Id, Id, Id"
                        ).ToList();
                    },
                    context
                );

            return userAccesses.FirstOrDefault();
        }

        private void ProcessSearchFilter(int idUser, int idCompetition)
        {
            base.AddFilterCriteria(ConditionType.Equal, "User_Competition", "IdUser", "idUser", idUser);
            base.AddFilterCriteria(ConditionType.Equal, "User_Competition", "IdCompetition", "idCompetition", idCompetition);
        }

        #endregion
    }
}
