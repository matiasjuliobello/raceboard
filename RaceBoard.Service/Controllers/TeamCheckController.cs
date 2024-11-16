using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.Team.Request;
using RaceBoard.DTOs.Team.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/teams")]
    [ApiController]
    public class TeamCheckController : AbstractController<TeamCheckController>
    {
        private readonly ITeamCheckManager _teamCheckManager;

        public TeamCheckController
            (
                IMapper mapper,
                ILogger<TeamCheckController> logger,
                ITranslator translator,
                ITeamCheckManager teamCheckManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _teamCheckManager = teamCheckManager;
        }

        [HttpGet("contestants/checks")]
        public ActionResult<PaginatedResultResponse<TeamContestantCheckResponse>> Get([FromQuery] TeamCheckSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<TeamCheckSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var data = _teamCheckManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<TeamContestantCheckResponse>>(data);

            return Ok(response);
        }

        [HttpGet("{id}/checks")]
        public ActionResult<TeamContestantCheckResponse> GetByIdTeam([FromRoute] int id)
        {
            var searchFilter = new TeamCheckSearchFilter()
            {
                Team = new Team() { Id = id }
            };

            var data = _teamCheckManager.Get(searchFilter);
            if (data.Results.Count() == 0)
                return NoContent();

            var teamCheck = data.Results.First();

            var response = _mapper.Map<TeamContestantCheckResponse>(teamCheck);

            return Ok(response);
        }

        [HttpPost("contestants/checks")]
        public ActionResult AddTeamCheck([FromBody] TeamCheckRequest teamCheckRequest)
        {
            var data = _mapper.Map<TeamContestantCheck>(teamCheckRequest);

            _teamCheckManager.Create(data);

            return Ok();
        }
    }
}
