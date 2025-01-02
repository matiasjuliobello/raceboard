using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Exceptions;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Common;
using RaceBoard.Domain;

namespace RaceBoard.Business.Managers
{
    public class RequestContextManager : IRequestContextManager
    {
        private readonly IHttpHeaderHelper _httpHeaderHelper;
        private readonly IUserManager _userManager;

        private readonly RequestContext _requestContext;

        public RequestContextManager(IHttpHeaderHelper httpHeaderHelper, IUserManager userManager)
        {
            _httpHeaderHelper = httpHeaderHelper;
            _userManager = userManager;

            _requestContext = InitializeContextProperty();
        }

        #region Public Methods

        public RequestContext GetContext()
        {
            return _requestContext;
        }

        public User GetUser()
        {
            if (_requestContext == null || string.IsNullOrEmpty(_requestContext.Username))
                throw new FunctionalException(Common.Enums.ErrorType.Unauthorized, "Authorizaton token has not been provided.");

            return _userManager.GetByUsername(_requestContext.Username);
        }

        #endregion

        #region Private Methods

        private RequestContext InitializeContextProperty()
        {
            if (_httpHeaderHelper == null)
                throw new FunctionalException(Common.Enums.ErrorType.InternalServerError, "No IHttpHeaderHelper concrete instance was provided");

            if (_userManager == null)
                throw new FunctionalException(Common.Enums.ErrorType.InternalServerError, "No IUserManager concrete instance was provided");

            return _httpHeaderHelper.GetContext();
        }

        #endregion
    }
}