using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers;
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
    public class TeamContestantController : AbstractController<TeamContestantController>
    {
        private readonly ITeamContestantManager _teamContestantManager;

        public TeamContestantController
            (
                IMapper mapper,
                ILogger<TeamContestantController> logger,
                ITranslator translator,
                ITeamContestantManager teamContestantManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _teamContestantManager = teamContestantManager;
        }

        [HttpGet("contestants")]
        public ActionResult<PaginatedResultResponse<TeamContestantResponse>> Get([FromQuery] TeamContestantSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<TeamContestantSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var data = _teamContestantManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<TeamContestantResponse>>(data);

            return Ok(response);
        }

        [HttpGet("contestants/{id}")]
        public ActionResult<TeamContestantResponse> Get([FromRoute] int id)
        {
            var data = _teamContestantManager.Get(id);

            var response = _mapper.Map<TeamContestantResponse>(data);

            return Ok(response);
        }

        [HttpPost("contestants")]
        public ActionResult Create([FromBody] TeamContestantRequest teamContestantRequest)
        {
            var data = _mapper.Map<TeamContestant>(teamContestantRequest);

            _teamContestantManager.Create(data);

            return Ok();
        }

        [HttpPut("contestants")]
        public ActionResult Update([FromBody] TeamContestantRequest teamContestantRequest)
        {
            var data = _mapper.Map<TeamContestant>(teamContestantRequest);

            _teamContestantManager.Update(data);

            return Ok();
        }

        [HttpDelete("contestants/{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _teamContestantManager.Delete(id);

            return Ok();
        }
    }
}