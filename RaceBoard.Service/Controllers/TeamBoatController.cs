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
    public class TeamBoatController : AbstractController<TeamBoatController>
    {
        private readonly ITeamBoatManager _teamBoatManager;

        public TeamBoatController
            (
                IMapper mapper,
                ILogger<TeamBoatController> logger,
                ITranslator translator,
                ITeamBoatManager teamBoatManager,
                ISessionHelper sessionHelper,
                IRequestContextManager requestContextManager
            ) : base(mapper, logger, translator, sessionHelper, requestContextManager)
        {
            _teamBoatManager = teamBoatManager;
        }

        [HttpGet("boats")]
        public ActionResult<PaginatedResultResponse<TeamBoatResponse>> Get([FromQuery] TeamBoatSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<TeamBoatSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var data = _teamBoatManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<TeamBoatResponse>>(data);

            return Ok(response);
        }

        [HttpGet("{id}/boats")]
        public ActionResult<TeamBoatResponse> GetByIdTeam([FromRoute] int id)
        {
            var searchFilter = new TeamBoatSearchFilter()
            {
                Team = new Team() { Id = id }
            };

            var data = _teamBoatManager.Get(searchFilter);
            if (data.Results.Count() == 0)
                return NoContent();

            var teamBoat = data.Results.First();

            var response = _mapper.Map<TeamBoatResponse>(teamBoat);
            
            return Ok(response);
        }

        [HttpGet("boats/{id}")]
        public ActionResult<TeamBoatResponse> Get([FromRoute] int id)
        {
            var data = _teamBoatManager.Get(id);

            var response = _mapper.Map<TeamBoatResponse>(data);

            return Ok(response);
        }

        [HttpPost("boats")]
        public ActionResult AddTeamBoat([FromBody] TeamBoatRequest teamBoatRequest)
        {
            var data = _mapper.Map<TeamBoat>(teamBoatRequest);

            _teamBoatManager.Create(data);

            return Ok();
        }

        [HttpPut("boats")]
        public ActionResult UpdateTeamBoat([FromBody] TeamBoatRequest teamBoatRequest)
        {
            var data = _mapper.Map<TeamBoat>(teamBoatRequest);

            _teamBoatManager.Update(data);

            return Ok();
        }

        [HttpDelete("boats/{id}")]
        public ActionResult RemoveTeamBoat([FromRoute] int id)
        {
            _teamBoatManager.Delete(id);

            return Ok();
        }
    }
}
