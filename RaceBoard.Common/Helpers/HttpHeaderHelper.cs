using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Common.Exceptions;

namespace RaceBoard.Common.Helpers
{
    public class HttpHeaderHelper : IHttpHeaderHelper
    {
        #region Private Members

        private RequestContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISecurityTicketHelper _securityTicketHelper;

        #endregion

        #region Constructors

        public HttpHeaderHelper(IHttpContextAccessor httpContextAccessor, ISecurityTicketHelper securityTicketHelper)
        {
            _httpContextAccessor = httpContextAccessor;
            _securityTicketHelper = securityTicketHelper;

            SetContext();
        }

        #endregion

        #region IHttpHeaderHelper implementation

        public RequestContext GetContext()
        {
            return _context;
        }

        public void SetContext()
        {
            var headers = _httpContextAccessor.HttpContext?.Request?.Headers;
            if (headers == null)
                throw new FunctionalException(Enums.ErrorType.Unauthorized, "Missing Request Headers");

            string language = GetValueFromHeaders(headers, CommonValues.HttpCustomHeaders.Language);

            var authorization = GetValueFromHeaders(headers, CommonValues.HttpCustomHeaders.Authorization);
            //if (authorization == null)
            //    throw new FunctionalException(Enums.ErrorType.Unauthorized, "Missing Access Token");

            string idStudio = GetValueFromHeaders(headers, CommonValues.HttpCustomHeaders.StudioId);
            //if (idStudio == null)
            //    throw new FunctionalException(Enums.ErrorType.Unauthorized, "Missing Studio Id");

            if (string.IsNullOrEmpty(authorization))
            {
                _context = new RequestContext
                {
                    IdStudio = idStudio,
                    Username = string.Empty,
                    Language = language
                };

                return;
            }

            string token = GetTokenFromAuthorizationHeader(authorization);

            var ticket = _securityTicketHelper.GetSecurityToken(token);

            string username = ticket.Id;

            _context = new RequestContext
            {
                IdStudio = idStudio,
                Username = username,
                Language = language
            };
        }

        #endregion

        #region Private Methods

        private string GetValueFromHeaders(IHeaderDictionary headers, string key)
        {
            if (!headers.ContainsKey(key))
                return null;

            StringValues value = headers[key];
            if (value.Count == 0)
                return null;

            return value.ToString();
        }

        private string GetTokenFromAuthorizationHeader(string authorization)
        {
            return authorization.Replace($"{CommonValues.AuthenticationTokenTypes.Bearer} ", "", StringComparison.InvariantCultureIgnoreCase);
        }

        #endregion
    }
}