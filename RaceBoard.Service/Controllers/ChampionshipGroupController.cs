using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Domain;
using RaceBoard.DTOs.Championship.Request;
using RaceBoard.DTOs.Championship.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/championships")]
    [ApiController]
    public class ChampionshipGroupController : AbstractController<ChampionshipGroupController>
    {
        private readonly IChampionshipManager _championshipManager;

        public ChampionshipGroupController
            (
                IMapper mapper,
                ILogger<ChampionshipGroupController> logger,
                ITranslator translator,
                IChampionshipManager championshipManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _championshipManager = championshipManager;
        }

        [HttpGet("{id}/groups")]
        public ActionResult<List<ChampionshipGroupResponse>> Get([FromRoute] int id)
        {
            var data = _championshipManager.GetGroups(id);

            var response = _mapper.Map<List<ChampionshipGroup>>(data);

            return Ok(response);
        }

        [HttpGet("groups/{id}")]
        public ActionResult<ChampionshipGroupResponse> GetById([FromRoute] int id)
        {
            var data = _championshipManager.GetGroup(id);

            var response = _mapper.Map<ChampionshipGroup>(data);

            return Ok(response);
        }

        [HttpGet("{id}/groups/teams")]
        public ActionResult<List<ChampionshipGroupResponse>> GetTeamCount([FromRoute] int id)
        {
            var data = _championshipManager.GetGroups(id);

            var response = _mapper.Map<List<ChampionshipGroup>>(data);

            return Ok(response);
        }

        [HttpPost("groups")]
        public ActionResult<int> Create(ChampionshipGroupRequest championshipGroupRequest)
        {
            var data = _mapper.Map<ChampionshipGroup>(championshipGroupRequest);

            _championshipManager.CreateGroup(data);

            return Ok(data.Id);
        }

        [HttpPut("groups")]
        public ActionResult Update(ChampionshipGroupRequest championshipGroupRequest)
        {
            var data = _mapper.Map<ChampionshipGroup>(championshipGroupRequest);

            _championshipManager.UpdateGroup(data);

            return Ok(data);
        }

        [HttpDelete("groups/{id}")]
        public ActionResult<int> Delete(int id)
        {
            _championshipManager.DeleteGroup(id);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}
