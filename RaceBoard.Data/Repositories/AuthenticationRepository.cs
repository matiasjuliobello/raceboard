using Microsoft.Extensions.Configuration;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Data.Repositories.Base.Abstract;
using RaceBoard.Data.Repositories.Base.Interfaces;
using RaceBoard.Data.Repositories.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Repositories
{
    public class AuthenticationRepository : AbstractRepository, IAuthenticationRepository
    {
        #region Private Members

        #endregion

        #region Constructors

        public AuthenticationRepository(IContextResolver contextResolver, IQueryBuilder queryBuilder) : base(contextResolver, queryBuilder)
        {
        }

        #endregion

        #region IAuthenticationRepository implementation

        public UserPassword Login(UserLogin userLogin, ITransactionalContext? context = null)
        {
            QueryBuilder.AddCommand("SELECT Id, Password FROM [User]");
            QueryBuilder.AddCondition("Username = @username");
            QueryBuilder.AddParameter("username", userLogin.Username);

            return GetSingleResult<UserPassword>(context);
        }

        #endregion
    }
}