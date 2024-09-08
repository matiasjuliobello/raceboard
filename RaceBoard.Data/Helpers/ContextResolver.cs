using Microsoft.Extensions.Configuration;
using RaceBoard.Common;
using RaceBoard.Data.Helpers.Interfaces;
using RaceBoard.Domain;

namespace RaceBoard.Data.Helpers
{
    public class ContextResolver : IContextResolver
    {
        #region Private Members

        private readonly string _connectionString = "";

        #endregion

        #region Constructors

        public ContextResolver(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionString_DB"];
        }

        #endregion

        #region IContextResolver implementation

        public string GetDatabaseConnection()
        {
            return GetConnectionString();
        }

        public User GetCurrentUser(RequestContext context)
        {
            return GetUser(context);
        }

        #endregion

        #region Private Methods

        private string GetConnectionString()
        {
            return _connectionString;
        }

        private User GetUser(RequestContext context)
        {
            //var user = _repository.GetUser(context);
            //if (user == null)
            //{
            //    throw new FunctionalException(ErrorType.NotFound, "User not found", setErrorsToBaseExceptionMessage: true);
            //}

            //return user;
            return null;
        }


        #endregion
    }
}
