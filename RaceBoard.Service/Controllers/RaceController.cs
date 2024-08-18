using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Domain;
using RaceBoard.DTOs.Race.Request;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/races")]
    [ApiController]
    public class RaceController : AbstractController<RaceController>
    {
        private readonly IRaceManager _raceManager;

        public RaceController
            (
                IMapper mapper,
                ILogger<RaceController> logger,
                ITranslator translator,
                IRaceManager raceManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _raceManager = raceManager;
        }

        [HttpPost()]
        public ActionResult<int> CreateRace(RaceRequest raceRequest)
        {
            var race = _mapper.Map<Race>(raceRequest);

            _raceManager.Create(race);

            return Ok(race.Id);
        }

        [HttpPut()]
        public ActionResult UpdateRace(RaceRequest raceRequest)
        {
            var race = _mapper.Map<Race>(raceRequest);

            _raceManager.Update(race);

            return Ok();
        }

        [HttpDelete("id")]
        public ActionResult DeleteRace(int id)
        {
            _raceManager.Delete(id);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}
