using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.Competition.Request;
using RaceBoard.DTOs.Competition.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/competitions")]
    [ApiController]
    public class CompetitionMemberController : AbstractController<CompetitionMemberController>
    {
        private readonly ICompetitionMemberManager _competitionMemberManager;

        public CompetitionMemberController
            (
                IMapper mapper,
                ILogger<CompetitionMemberController> logger,
                ITranslator translator,
                ICompetitionMemberManager competitionMemberManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _competitionMemberManager = competitionMemberManager;
        }

        [HttpGet("{id}/members")]
        public ActionResult<PaginatedResultResponse<CompetitionMemberResponse>> Get([FromRoute] int id, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var searchFilter = new CompetitionMemberSearchFilter()
            {
                Competition = new Competition () {  Id = id }
            };
            var data = _competitionMemberManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<CompetitionMemberResponse>>(data);

            return Ok(response);
        }

        [HttpGet("{id}/members/pending")]
        public ActionResult<PaginatedResultResponse<CompetitionMemberInvitationResponse>> GetPendingInvitations([FromRoute] int id, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var data = _competitionMemberManager.GetMemberInvitations(id, isPending: true, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<CompetitionMemberInvitationResponse>>(data);

            return Ok(response);
        }

        [HttpPost("members")]
        public ActionResult AddMemberInvitation([FromBody] CompetitionMemberInvitationRequest competitionMemberInvitationRequest)
        {
            var data = _mapper.Map<CompetitionMemberInvitation>(competitionMemberInvitationRequest);

            data.RequestUser = base.GetUserFromRequestContext();

            _competitionMemberManager.AddInvitation(data);

            return Ok();
        }

        [HttpPut("members")]
        public ActionResult UpdateMemberInvitation([FromBody] CompetitionMemberInvitationRequest competitionMemberInvitationRequest)
        {
            var data = _mapper.Map<CompetitionMemberInvitation>(competitionMemberInvitationRequest);

            data.User = base.GetUserFromRequestContext();

            _competitionMemberManager.UpdateInvitation(data);

            return Ok();
        }

        [HttpDelete("members/{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _competitionMemberManager.Remove(id);

            return Ok();
        }

        [HttpDelete("members/pending/{id}")]
        public ActionResult DeleteMemberInvitation([FromRoute] int id)
        {
            _competitionMemberManager.RemoveInvitation(id);

            return Ok();
        }
    }
}