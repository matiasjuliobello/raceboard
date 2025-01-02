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
                IRequestContextManager requestContextManager
            ) : base(mapper, logger, translator, sessionHelper, requestContextManager)
        {
            _userSettingsManager = userSettingsManager;
        }

        #endregion

        #region Public Methods

        [HttpGet("{id}/settings")]
        public ActionResult<UserSettingsResponse> Get(int id)
        {
            var userSettings = _userSettingsManager.Get(id);

            var response = _mapper.Map<UserSettingsResponse>(userSettings);

            return Ok(response);
        }        

        [HttpPost("settings")]
        public ActionResult Create(UserSettingsRequest userSettingsRequest)
        {
            var data = _mapper.Map<UserSettings>(userSettingsRequest);

            data.User = base.GetUserFromRequestContext();

            _userSettingsManager.Create(data);

            return Ok(data.Id);
        }

        [HttpPut("settings")]
        public ActionResult Update(UserSettingsRequest userSettingsRequest)
        {
            var data = _mapper.Map<UserSettings>(userSettingsRequest);

            data.User = base.GetUserFromRequestContext();

            _userSettingsManager.Update(data);

            return Ok();
        }

        [HttpDelete("settings/{id}")]
        public ActionResult Delete(int id)
        {
            var data = _userSettingsManager.Delete(id);

            return Ok();
        }

        #endregion
    }
}