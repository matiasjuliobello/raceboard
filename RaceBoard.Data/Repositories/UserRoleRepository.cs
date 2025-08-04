using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class UserRoleRepository : AbstractRepository, IUserRoleRepository
    {
        #region Private Members

        #endregion

        #region Constructors

        public UserRoleRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IUserRoleRepository implementation

        public ITransactionalContext GetTransactionalContext(TransactionContextScope scope = TransactionContextScope.Internal)
        {
            return base.GetTransactionalContext(scope);
        }

        public void ConfirmTransactionalContext(ITransactionalContext context)
        {
            base.ConfirmTransactionalContext(context);
        }

        public void CancelTransactionalContext(ITransactionalContext context)
        {
            base.CancelTransactionalContext(context);
        }

        public bool Exists(int id, ITransactionalContext? context = null)
        {
            return base.Exists(id, "User_Role", "Id", context);
        }

        public bool ExistsDuplicate(UserRole userRole, ITransactionalContext? context = null)
        {
            //string condition = "[IdChampionship] = @idChampionship AND [IdRaceClass] = @idRaceClass";

            //string existsQuery = base.GetExistsDuplicateQuery("[Team]", condition, "Id", "@id");

            //QueryBuilder.AddCommand(existsQuery);
            //QueryBuilder.AddParameter("idChampionship", team.Championship.Id);
            //QueryBuilder.AddParameter("idRaceClass", team.RaceClass.Id);
            //QueryBuilder.AddParameter("id", team.Id);

            //return base.Execute<bool>(context);

            return false;
        }

        public void Create(UserRole userRole, ITransactionalContext? context = null)
        {
            this.CreateUserRole(userRole, context);
        }

        #endregion

        #region Private Methods

        private void CreateUserRole(UserRole userRole, ITransactionalContext? context = null)
        {
            string sql = @" INSERT INTO [User_Role]
                                ( IdUser, IdRole )
                            VALUES
                                ( @idUser, @idRole )";

            QueryBuilder.AddCommand(sql);

            QueryBuilder.AddParameter("idUser", userRole.User.Id);
            QueryBuilder.AddParameter("idRole", userRole.Role.Id);

            QueryBuilder.AddReturnLastInsertedId();

            base.Execute<int>(context);
        }

        #endregion
    }
}
