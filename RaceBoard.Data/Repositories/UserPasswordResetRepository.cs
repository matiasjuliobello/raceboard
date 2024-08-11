using Dapper;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Interfaces;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;
using SqlBoolean = RaceBoard.Data.Constants.SqlBoolean;
using RaceBoard.Data.Repositories.Base.Abstract;
using Microsoft.Extensions.Configuration;

namespace RaceBoard.Data.Repositories
{
    public class UserPasswordResetRepository : AbstractRepository, IUserPasswordResetRepository
    {
        #region Private Members

        #endregion

        #region Constructors

        public UserPasswordResetRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IUserPasswordResetRepository implementation

        public ITransactionalContext GetTransactionalContext()
        {
            return base.GetTransactionalContext();
        }

        public UserPasswordReset GetByToken(string token, bool? isUsed = null, bool? isActive = null, ITransactionalContext? context = null)
        {
            string sql = $@"SELECT
                                [UserPasswordReset].Id [Id], 
                                [UserPasswordReset].Token [Token],
                                [UserPasswordReset].RequestDate [RequestDate],
                                [UserPasswordReset].ExpirationDate [ExpirationDate],
                                [UserPasswordReset].UseDate [UseDate],
                                [UserPasswordReset].IsUsed [IsUsed],
                                [UserPasswordReset].IsActive [IsActive],
                                [User].Id [Id]
                            FROM [UserPasswordReset]
                            INNER JOIN [User] ON [User].Id = [UserPasswordReset].IdUser";

            QueryBuilder.AddCommand(sql);
            
            QueryBuilder.AddCondition($"[UserPasswordReset].Token = @token");
            QueryBuilder.AddParameter("token", token);

            if (isUsed.HasValue)
            {
                QueryBuilder.AddCondition($"[UserPasswordReset].IsUsed = @isUsed");
                QueryBuilder.AddParameter("isUsed", isUsed.Value);
            }

            if (isActive.HasValue)
            {
                QueryBuilder.AddCondition($"[UserPasswordReset].IsActive = @isActive");
                QueryBuilder.AddParameter("isActive", isActive.Value);
            }

            var items = new List<UserPasswordReset>();

            base.GetReader
            (
                (x) =>
                {
                    items = x.Read<UserPasswordReset, User, UserPasswordReset>
                    (
                        (userPasswordReset, user) =>
                        {
                            userPasswordReset.User = user;

                            return userPasswordReset;
                        },
                        splitOn: "Id, Id"
                    ).AsList();
                },
                context
            );

            return items.FirstOrDefault();
        }

        public void Create(UserPasswordReset userPasswordReset, ITransactionalContext? context = null)
        {
            ITransactionalContext transaction = base.GetTransactionalContext();

            try
            {
                QueryBuilder.AddCommand($"UPDATE [UserPasswordReset] SET IsActive = {SqlBoolean.False}");

                QueryBuilder.AddParameter("isActive", SqlBoolean.True);
                QueryBuilder.AddCondition($"IsActive = @isActive");
                QueryBuilder.AddParameter("idUser", userPasswordReset.User.Id);
                QueryBuilder.AddCondition("IdUser = @idUser");

                base.ExecuteAndGetRowsAffected(transaction);

                string sql = @" INSERT INTO [UserPasswordReset]
                                ( IdUser, Token, RequestDate, ExpirationDate, UseDate, IsUsed, IsActive )
                            VALUES
                                ( @idUser, @token, @requestDate, @expirationDate, @useDate, @isUsed, @isActive )";

                QueryBuilder.AddCommand(sql);
                QueryBuilder.AddParameter("idUser", userPasswordReset.User.Id);
                QueryBuilder.AddParameter("token", userPasswordReset.Token);
                QueryBuilder.AddParameter("requestDate", userPasswordReset.RequestDate);
                QueryBuilder.AddParameter("expirationDate", userPasswordReset.ExpirationDate);
                QueryBuilder.AddParameter("useDate", null);
                QueryBuilder.AddParameter("isUsed", false);
                QueryBuilder.AddParameter("isActive", true);

                base.ExecuteAndGetRowsAffected(transaction);

                transaction.Confirm();
            }
            catch (Exception)
            {
                transaction.Cancel();

                throw;
            }
        }

        public void Update(UserPasswordReset userPasswordReset, ITransactionalContext? context = null)
        {
            QueryBuilder.AddCommand($"UPDATE [UserPasswordReset] SET IsActive = {SqlBoolean.False}, IsUsed = {SqlBoolean.True}, UseDate = @useDate");
            QueryBuilder.AddParameter("idUser", userPasswordReset.User.Id);
            QueryBuilder.AddParameter("useDate", userPasswordReset.UseDate.Value);
            QueryBuilder.AddCondition("IdUser = @idUser");
            QueryBuilder.AddParameter("isActive", SqlBoolean.True);
            QueryBuilder.AddCondition("IsActive = @isActive");

            base.ExecuteAndGetRowsAffected(context);
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
