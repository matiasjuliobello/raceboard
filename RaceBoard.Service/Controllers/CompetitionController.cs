using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Domain;
using RaceBoard.DTOs.Competition.Request;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/competitions")]
    [ApiController]
    public class CompetitionController : AbstractController<CompetitionController>
    {
        private readonly ICompetitionManager _competitionManager;

        public CompetitionController
            (
                IMapper mapper,
                ILogger<CompetitionController> logger,
                ITranslator translator,
                ICompetitionManager competitionManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _competitionManager = competitionManager;
        }

        [HttpPost()]
        public ActionResult<int> CreateCompetition(CompetitionRequest competitionRequest)
        {
            var competition = _mapper.Map<Competition>(competitionRequest);

            _competitionManager.Create(competition);

            return Ok(competition.Id);
        }

        [HttpPut()]
        public ActionResult UpdateCompetition(CompetitionRequest competitionRequest)
        {
            var competition = _mapper.Map<Competition>(competitionRequest);

            _competitionManager.Update(competition);

            return Ok();
        }

        [HttpDelete("id")]
        public ActionResult DeleteCompetition(int id)
        {
            _competitionManager.Delete(id);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}
