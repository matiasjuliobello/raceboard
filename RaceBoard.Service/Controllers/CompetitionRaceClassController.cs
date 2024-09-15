using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Domain;
using RaceBoard.DTOs.Competition.Request;
using RaceBoard.DTOs.Competition.Response;
using RaceBoard.DTOs.RaceClass.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/competitions")]
    [ApiController]
    public class CompetitionRaceClassController : AbstractController<CompetitionRaceClassController>
    {
        private readonly ICompetitionManager _competitionManager;

        public CompetitionRaceClassController
            (
                IMapper mapper,
                ILogger<CompetitionRaceClassController> logger,
                ITranslator translator,
                ICompetitionManager competitionManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _competitionManager = competitionManager;
        }

        [HttpGet("{id}/race-classes")]
        public ActionResult<List<CompetitionRaceClassResponse>> Get([FromRoute] int id)
        {
            var data = _competitionManager.GetRaceClasses(id);

            var response = _mapper.Map<List<RaceClassResponse>>(data.Select(x => x.RaceClass).ToList());

            return Ok(response);
        }

        [HttpPost("race-classes")]
        public ActionResult Set(CompetitionRaceClassRequest competitionRaceClassRequest)
        {
            var competitionRaceClasses = _mapper.Map<List<CompetitionRaceClass>>(competitionRaceClassRequest);

            _competitionManager.SetRaceClasses(competitionRaceClasses);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}
