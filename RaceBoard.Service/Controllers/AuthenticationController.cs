using AutoMapper;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common;
using RaceBoard.Common.Exceptions;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Domain;
using RaceBoard.DTOs.Authentication.Response;
using RaceBoard.DTOs.Authentication_SSO.Request;
using RaceBoard.DTOs.Authentication_SSO.Response;
using RaceBoard.DTOs.Person.Response;
using RaceBoard.DTOs.User.Request;
using RaceBoard.DTOs.User.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : AbstractController<AuthenticationController>
    {
        #region Private Members

        private readonly IConfiguration _configuration;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly ISecurityTicketHelper _securityTicketHelper;
        private readonly IUserManager _userManager;
        private readonly IPersonManager _personManager;

        #endregion

        #region Constructors

        public AuthenticationController
            (
                IConfiguration configuration,
                IMapper mapper,
                ILogger<AuthenticationController> logger,
                ITranslator translator,
                IAuthenticationManager authenticationManager,
                IUserManager userManager,
                IPersonManager personManager,
                ISecurityTicketHelper securityTicketHelper,
                ISessionHelper sessionHelper,
                IRequestContextManager requestContextManager
            ) : base(mapper, logger, translator, sessionHelper, requestContextManager)
        {
            _configuration = configuration;
            _authenticationManager = authenticationManager;
            _securityTicketHelper = securityTicketHelper;
            _userManager = userManager;
            _personManager = personManager;
        }

        #endregion

        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult<UserLoginResponse> Login([FromBody] UserLoginRequest userLoginRequest)
        {
            var userLogin = _mapper.Map<UserLogin>(userLoginRequest);

            _authenticationManager.Login(userLogin);

            return FinalizeLoginProcess(id: userLogin.Username, username: userLogin.Username, email: null);
        }

        [HttpPost("login/sso")]
        [AllowAnonymous]
        public ActionResult<UserLoginResponse> LoginSSO([FromBody] SsoAuthCodeRequest ssoAuthCodeRequest)
        {
            string url = _configuration["Google_SSO_Url"];
            var redirectUrl = _configuration["Google_SSO_RedirectUrl"];
            var clientId = _configuration["Google_SSO_ClientId"];
            var clientSecret = _configuration["Google_SSO_ClientSecret"];

            string authCode = ssoAuthCodeRequest.Code;

            var tokenRequest = new Dictionary<string, string>
            {
                { "code", authCode },
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "redirect_uri", redirectUrl },
                { "grant_type", "authorization_code" }
            };

            var httpClient = new HttpClient();

            var googleSsoLogin = httpClient.PostAsync(url, new FormUrlEncodedContent(tokenRequest)).Result;
            if (!googleSsoLogin.IsSuccessStatusCode)
                return Unauthorized(new { error = "Error exchanging code with Google" });

            var json = googleSsoLogin.Content.ReadAsStringAsync().Result;
            if (json == null)
                return Unauthorized(new { error = "Error reading response from Google" });

            var tokenData = JsonConvert.DeserializeObject<GoogleTokenResponse>(json);
            if (tokenData == null || string.IsNullOrEmpty(tokenData.id_token))
                return Unauthorized(new { error = "Invalid token data from Google" });

            var googleJsonWebValidationSettings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { clientId }
            };
            var payload = GoogleJsonWebSignature.ValidateAsync(tokenData.id_token, googleJsonWebValidationSettings).Result;

            return FinalizeLoginProcess(id: tokenData.id_token, username: null, email: payload.Email);
        }

        #region Private Methods

        private ActionResult<UserLoginResponse> FinalizeLoginProcess(string id, string? username = null, string? email = null)
        {
            User user = null;

            if (!string.IsNullOrEmpty(username))
                user = _userManager.GetByUsername(username);

            if (!string.IsNullOrEmpty(email))
                user = _userManager.GetByEmailAddress(email);

            if (user == null)
                throw new FunctionalException(Common.Enums.ErrorType.NotFound, "User not found");

            var accessToken = _securityTicketHelper.GenerateToken(id, user.Email);

            var accessTokenResponse = _mapper.Map<AccessToken, AccessTokenResponse>(accessToken);
            var userResponse = _mapper.Map<User, UserSimpleResponse>(user);

            var response = new UserLoginResponse()
            {
                AccessToken = accessTokenResponse,
                User = userResponse
            };

            var person = _personManager.GetByIdUser(user.Id);
            if (person != null)
            {
                person.User = null;
                response.Person = _mapper.Map<Person, PersonResponse>(person);
            }

            return Ok(response);
        }

        #endregion
    }
}