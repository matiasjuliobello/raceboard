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
    public class TeamMemberController : AbstractController<TeamMemberController>
    {
        private readonly ITeamMemberManager _teamMemberManager;

        public TeamMemberController
            (
                IMapper mapper,
                ILogger<TeamMemberController> logger,
                ITranslator translator,
                ITeamMemberManager teamMemberManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _teamMemberManager = teamMemberManager;
        }

        [HttpGet("members")]
        public ActionResult<PaginatedResultResponse<TeamMemberResponse>> Get([FromQuery] TeamMemberSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<TeamMemberSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var data = _teamMemberManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<TeamMemberResponse>>(data);

            return Ok(response);
        }

        [HttpGet("members/{id}")]
        public ActionResult<TeamMemberResponse> Get([FromRoute] int id)
        {
            var data = _teamMemberManager.Get(id);

            var response = _mapper.Map<TeamMemberResponse>(data);

            return Ok(response);
        }

        [HttpPost("members")]
        public ActionResult Create([FromBody] TeamMemberRequest teamMemberRequest)
        {
            var data = _mapper.Map<TeamMember>(teamMemberRequest);

            _teamMemberManager.Create(data);

            return Ok();
        }

        [HttpPut("members")]
        public ActionResult Update([FromBody] TeamMemberRequest teamMemberRequest)
        {
            var data = _mapper.Map<TeamMember>(teamMemberRequest);

            _teamMemberManager.Update(data);

            return Ok();
        }

        [HttpDelete("members/{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _teamMemberManager.Delete(id);

            return Ok();
        }
    }
}