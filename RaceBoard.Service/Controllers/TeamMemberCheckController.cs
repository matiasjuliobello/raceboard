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
    public class TeamMemberCheckController : AbstractController<TeamMemberCheckController>
    {
        private readonly ITeamCheckManager _teamCheckManager;

        public TeamMemberCheckController
            (
                IMapper mapper,
                ILogger<TeamMemberCheckController> logger,
                ITranslator translator,
                ITeamCheckManager teamCheckManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _teamCheckManager = teamCheckManager;
        }

        [HttpGet("members/checks")]
        public ActionResult<PaginatedResultResponse<TeamMemberCheckResponse>> Get([FromQuery] TeamCheckSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<TeamCheckSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var data = _teamCheckManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<TeamMemberCheckResponse>>(data);

            return Ok(response);
        }

        [HttpPost("members/checks")]
        public ActionResult RegisterTeamMemberCheck([FromBody] TeamCheckRequest teamMemberCheckRequest)
        {
            var teamMemberCheck = _mapper.Map<TeamMemberCheck>(teamMemberCheckRequest);

            teamMemberCheck.TeamMember = new TeamMember()
            {
                Person = new Person()
                { 
                    Id = teamMemberCheckRequest.IdPerson 
                } 
            };

            _teamCheckManager.Create(teamMemberCheck);

            return Ok(teamMemberCheck.Id);
        }
    }
}
