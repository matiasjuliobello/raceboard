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
                IRequestContextManager requestContextManager
            ) : base(mapper, logger, translator, sessionHelper, requestContextManager)
        {
            _teamMemberManager = teamMemberManager;
        }

        [HttpGet("{id}/members")]
        public ActionResult<PaginatedResultResponse<TeamMemberResponse>> Get([FromRoute] int id, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var searchFilter = new TeamMemberSearchFilter()
            {
                Team = new Team() {  Id = id }
            };
            var data = _teamMemberManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<TeamMemberResponse>>(data);

            return Ok(response);
        }

        [HttpGet("{id}/members/pending")]
        public ActionResult<PaginatedResultResponse<TeamMemberInvitationResponse>> GetPendingInvitations([FromRoute] int id, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var data = _teamMemberManager.GetMemberInvitations(id, isPending: true, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<TeamMemberInvitationResponse>>(data);

            return Ok(response);
        }

        [HttpPost("members")]
        public ActionResult AddMemberInvitation([FromBody] TeamMemberInvitationRequest teamMemberInvitationRequest)
        {
            var data = _mapper.Map<TeamMemberInvitation>(teamMemberInvitationRequest);

            _teamMemberManager.CreateInvitation(data);

            return Ok();
        }

        [HttpPut("members")]
        public ActionResult UpdateMemberInvitation([FromBody] TeamMemberInvitationRequest teamMemberInvitationRequest)
        {
            var data = _mapper.Map<TeamMemberInvitation>(teamMemberInvitationRequest);

            _teamMemberManager.UpdateInvitation(data);

            return Ok();
        }

        [HttpDelete("members/{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _teamMemberManager.Remove(id);

            return Ok();
        }

        [HttpDelete("members/pending/{id}")]
        public ActionResult DeleteMemberInvitation([FromRoute] int id)
        {
            _teamMemberManager.RemoveInvitation(id);

            return Ok();
        }
    }
}