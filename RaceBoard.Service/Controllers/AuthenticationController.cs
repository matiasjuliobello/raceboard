using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Domain;
using RaceBoard.DTOs.User.Request;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.DTOs.Authentication.Response;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Common;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class AuthenticationController : AbstractController<AuthenticationController>
    {
        #region Private Members

        private readonly IAuthenticationManager _authenticationManager;
        private readonly ISecurityTicketHelper _securityTicketHelper;

        #endregion

        #region Constructors

        public AuthenticationController
            (
                IMapper mapper,
                ILogger<AuthenticationController> logger,
                ITranslator translator,
                IAuthenticationManager authenticationManager,
                ISecurityTicketHelper securityTicketHelper,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _authenticationManager = authenticationManager;
            _securityTicketHelper = securityTicketHelper;
        }

        #endregion

        [HttpPost]
        [AllowAnonymous]
        public ActionResult<string> Login([FromBody] UserLoginRequest userLoginRequest)
        {
            var userLogin = _mapper.Map<UserLogin>(userLoginRequest);

            _authenticationManager.Login(userLogin);

            var accessToken = _securityTicketHelper.CreateToken(userLogin.Username);

            var response = _mapper.Map<AccessToken, AccessTokenResponse>(accessToken);
            
            return Ok(response);
        }
    }
}