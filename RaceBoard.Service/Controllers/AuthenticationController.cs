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
using RaceBoard.DTOs.User.Response;

namespace RaceBoard.Service.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : AbstractController<AuthenticationController>
    {
        #region Private Members

        private readonly IAuthenticationManager _authenticationManager;
        private readonly ISecurityTicketHelper _securityTicketHelper;
        private readonly IUserManager _userManager;

        #endregion

        #region Constructors

        public AuthenticationController
            (
                IMapper mapper,
                ILogger<AuthenticationController> logger,
                ITranslator translator,
                IAuthenticationManager authenticationManager,
                IUserManager userManager,
                ISecurityTicketHelper securityTicketHelper,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _authenticationManager = authenticationManager;
            _securityTicketHelper = securityTicketHelper;
            _userManager = userManager;
        }

        #endregion

        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult<UserLoginResponse> Login([FromBody] UserLoginRequest userLoginRequest)
        {
            var userLogin = _mapper.Map<UserLogin>(userLoginRequest);

            _authenticationManager.Login(userLogin);

            var accessToken = _securityTicketHelper.CreateToken(userLogin.Username);

            var user = _userManager.GetByUsername(userLogin.Username);

            var accessTokenResponse = _mapper.Map<AccessToken, AccessTokenResponse>(accessToken);
            var userResponse = _mapper.Map<User, UserSimpleResponse>(user);

            var response = new UserLoginResponse()
            {
                AccessToken = accessTokenResponse,
                User = userResponse
            };
            
            return Ok(response);
        }
    }
}