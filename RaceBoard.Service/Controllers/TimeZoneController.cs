using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Service.Attributes;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.DTOs.User.Response.Settings;
using TimeZone = RaceBoard.Domain.TimeZone;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/timezones")]
    [ApiController]
    public class TimeZoneController : AbstractController<TimeZoneController>
    {
        private readonly ITimeZoneManager _timeZoneManager;

        public TimeZoneController
            (
                IMapper mapper,
                ILogger<TimeZoneController> logger,
                ITranslator translator,
                ITimeZoneManager timeZoneManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _timeZoneManager = timeZoneManager;
        }

        [HttpGet()]
        public ActionResult<List<TimeZoneResponse>> Get()
        {
            var data = _timeZoneManager.Get();

            var response = _mapper.Map<List<TimeZone>, List<TimeZoneResponse>>(data);

            return Ok(response);
        }

        #region Private Methods

        #endregion
    }
}
