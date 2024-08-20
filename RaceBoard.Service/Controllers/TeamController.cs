using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Domain;
using RaceBoard.DTOs.Boat.Request;
using RaceBoard.DTOs.Team;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/teams")]
    [ApiController]
    public class TeamController : AbstractController<TeamController>
    {
        private readonly ITeamManager _teamManager;

        public TeamController
            (
                IMapper mapper,
                ILogger<TeamController> logger,
                ITranslator translator,
                ITeamManager teamManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _teamManager = teamManager;
        }

        [HttpPost()]
        public ActionResult<int> CreateTeam(TeamRequest teamRequest)
        {
            var team = _mapper.Map<Team>(teamRequest);

            _teamManager.Create(team);

            return Ok(team.Id);
        }

        [HttpPut()]
        public ActionResult UpdateTeam(TeamRequest teamRequest)
        {
            var team = _mapper.Map<Team>(teamRequest);

            _teamManager.Update(team);

            return Ok();
        }

        [HttpDelete("id")]
        public ActionResult DeleteTeam(int id)
        {
            _teamManager.Delete(id);

            return Ok();
        }

        [HttpPost("boats")]
        public ActionResult SetTeamBoat([FromBody] TeamBoatRequest teamBoatRequest)
        {
            var teamBoat = _mapper.Map<TeamBoat>(teamBoatRequest);

            _teamManager.SetBoat(teamBoat);

            return Ok();
        }

        [HttpPost("contestants")]
        public ActionResult SetTeamContestants([FromBody] TeamContestantsRequest teamContestantsRequest)
        {
            var teamContestants = _mapper.Map<List<TeamContestant>>(teamContestantsRequest.Contestants);

            teamContestants.ForEach(x => x.Team = new Team() { Id = teamContestantsRequest.IdTeam });

            _teamManager.SetContestants(teamContestants);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}
