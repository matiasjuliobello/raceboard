using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Domain;
using RaceBoard.DTOs.Competition.Request;
using RaceBoard.DTOs.Competition.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/competitions")]
    [ApiController]
    public class CompetitionGroupController : AbstractController<CompetitionGroupController>
    {
        private readonly ICompetitionManager _competitionManager;

        public CompetitionGroupController
            (
                IMapper mapper,
                ILogger<CompetitionGroupController> logger,
                ITranslator translator,
                ICompetitionManager competitionManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _competitionManager = competitionManager;
        }

        [HttpGet("{id}/groups")]
        public ActionResult<List<CompetitionGroupResponse>> Get([FromRoute] int id)
        {
            var data = _competitionManager.GetGroups(id);

            var response = _mapper.Map<List<CompetitionGroup>>(data);

            return Ok(response);
        }

        [HttpGet("groups/{id}")]
        public ActionResult<CompetitionGroupResponse> GetById([FromRoute] int id)
        {
            var data = _competitionManager.GetGroup(id);

            var response = _mapper.Map<CompetitionGroup>(data);

            return Ok(response);
        }

        [HttpGet("{id}/groups/teams")]
        public ActionResult<List<CompetitionGroupResponse>> GetTeamCount([FromRoute] int id)
        {
            var data = _competitionManager.GetGroups(id);

            var response = _mapper.Map<List<CompetitionGroup>>(data);

            return Ok(response);
        }

        [HttpPost("groups")]
        public ActionResult<int> Create(CompetitionGroupRequest competitionGroupRequest)
        {
            var data = _mapper.Map<CompetitionGroup>(competitionGroupRequest);

            _competitionManager.CreateGroup(data);

            return Ok(data.Id);
        }

        [HttpPut("groups")]
        public ActionResult Update(CompetitionGroupRequest competitionGroupRequest)
        {
            var data = _mapper.Map<CompetitionGroup>(competitionGroupRequest);

            _competitionManager.UpdateGroup(data);

            return Ok(data);
        }

        [HttpDelete("groups/{id}")]
        public ActionResult<int> Delete(int id)
        {
            _competitionManager.DeleteGroup(id);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}
