using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.DTOs.User.Request.Password;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/user-password-reset")]
    [ApiController]
    public class UserPasswordResetController : AbstractController<UserPasswordResetController>
    {
        private readonly IUserPasswordResetManager _userPasswordResetManager;

        public UserPasswordResetController
            (
                IMapper mapper, 
                ILogger<UserPasswordResetController> logger,
                ITranslator translator,
                IUserPasswordResetManager userPasswordResetManager,
                ISessionHelper sessionHelper,
                IRequestContextManager requestContextManager
            ) : base(mapper, logger, translator, sessionHelper, requestContextManager)
        {
            _userPasswordResetManager = userPasswordResetManager;
        }

        /// <summary>
        /// Creates a password reset action.
        /// </summary>
        /// <returns></returns>
        [HttpPost()]
        [AllowAnonymous]
        public ActionResult CreatePasswordReset([FromBody] UserPasswordResetRequest userPasswordResetRequest)
        {
            _userPasswordResetManager.Create(userPasswordResetRequest.EmailAddress);

            return Ok();
        }

        /// <summary>
        /// Updates a user password given a password reset token.
        /// </summary>
        /// <param name="userPasswordResetRequest"></param>
        /// <returns></returns>
        [HttpPut()]
        [AllowAnonymous]
        public ActionResult UpdatePasswordReset([FromBody] UserPasswordResetRequest userPasswordResetRequest)
        {
            _userPasswordResetManager.Update(userPasswordResetRequest.Token, userPasswordResetRequest.Password);

            return Ok();
        }

    }
}
