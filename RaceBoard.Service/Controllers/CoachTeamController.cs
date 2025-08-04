using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.Coach.Request;
using RaceBoard.DTOs.Coach.Response;
using RaceBoard.DTOs.Team.Request;
using RaceBoard.DTOs.Team.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/coaches")]
    [ApiController]
    public class CoachTeamController : AbstractController<CoachTeamController>
    {
        private readonly ICoachTeamManager _coachTeamManager;

        public CoachTeamController
            (
                IMapper mapper,
                ILogger<CoachTeamController> logger,
                ITranslator translator,
                ICoachTeamManager coachTeamManager,
                ISessionHelper sessionHelper,
                IRequestContextManager requestContextManager
            ) : base(mapper, logger, translator, sessionHelper, requestContextManager)
        {
            _coachTeamManager = coachTeamManager;
        }

        [HttpGet("{id}/teams")]
        public ActionResult<PaginatedResultResponse<CoachTeamResponse>> Get([FromRoute] int id, [FromQuery] CoachTeamSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<CoachTeamSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            searchFilter.Coach = new Coach() { Id = id };

            var coachTeams = _coachTeamManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<CoachTeamResponse>>(coachTeams);

            return Ok(response);
        }

        [HttpGet("teams/{id}")]
        public ActionResult<CoachTeamResponse> Get([FromRoute] int id)
        {
            var data = _coachTeamManager.Get(id);

            var response = _mapper.Map<CoachTeamResponse>(data);

            return Ok(response);
        }

        [HttpPost("teams")]
        public ActionResult CreateCoachTeam([FromBody] CoachTeamRequest coachTeamRequest)
        {
            var data = _mapper.Map<CoachTeam>(coachTeamRequest);

            _coachTeamManager.Create(data);

            return Ok();
        }

        [HttpPut("teams")]
        public ActionResult UpdateCoachTeam([FromBody] CoachTeamRequest coachTeamRequest)
        {
            var data = _mapper.Map<CoachTeam>(coachTeamRequest);

            _coachTeamManager.Update(data);

            return Ok();
        }

        [HttpDelete("teams/{id}")]
        public ActionResult RemoveCoachTeam([FromRoute] int id)
        {
            //_coachTeamManager.Delete(id);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}
