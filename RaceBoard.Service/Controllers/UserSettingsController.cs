using AutoMapper;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Domain;
using RaceBoard.DTOs.User.Request;
using RaceBoard.DTOs.User.Response.Settings;
using RaceBoard.Service.Attributes;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace RaceBoard.Service.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserSettingsController : AbstractController<UserSettingsController>
    {
        #region Private Members

        private readonly IUserSettingsManager _userSettingsManager;

        #endregion

        #region Constructors

        public UserSettingsController
            (
                IMapper mapper, 
                ILogger<UserSettingsController> logger,
                ITranslator translator,
                IUserSettingsManager userSettingsManager,
                ISessionHelper sessionHelper,                
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _userSettingsManager = userSettingsManager;
        }

        #endregion

        #region Public Methods

        [HttpGet("{id}/settings")]
        public ActionResult<UserSettingsResponse> GetUserSettings(int id)
        {
            var userSettings = _userSettingsManager.Get(id);

            var response = _mapper.Map<UserSettingsResponse>(userSettings);

            return Ok(response);
        }        

        [HttpPost("settings")]
        public ActionResult CreateUserSettings(UserSettingsRequest userSettingsRequest)
        {
            var userSettings = _mapper.Map<UserSettings>(userSettingsRequest);

            _userSettingsManager.Create(userSettings);

            return Ok(userSettings.Id);
        }

        [HttpPut("settings")]
        public ActionResult UpdateUserSettings(UserSettingsRequest userSettingsRequest)
        {
            var userSettings = _mapper.Map<UserSettings>(userSettingsRequest);

            _userSettingsManager.Update(userSettings);

            return Ok();
        }

        [HttpDelete("settings/{id}")]
        public ActionResult DeleteUserSettings(int id)
        {
            var userSettings = _userSettingsManager.Delete(id);

            return Ok();
        }

        #endregion
    }
}