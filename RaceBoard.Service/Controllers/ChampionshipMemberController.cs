using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.Championship.Request;
using RaceBoard.DTOs.Championship.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/championships")]
    [ApiController]
    public class ChampionshipMemberController : AbstractController<ChampionshipMemberController>
    {
        private readonly IChampionshipMemberManager _championshipMemberManager;

        public ChampionshipMemberController
            (
                IMapper mapper,
                ILogger<ChampionshipMemberController> logger,
                ITranslator translator,
                IChampionshipMemberManager championshipMemberManager,
                ISessionHelper sessionHelper,
                IRequestContextManager requestContextManager
            ) : base(mapper, logger, translator, sessionHelper, requestContextManager)
        {
            _championshipMemberManager = championshipMemberManager;
        }

        [HttpGet("{id}/members")]
        public ActionResult<PaginatedResultResponse<ChampionshipMemberResponse>> Get([FromRoute] int id, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var searchFilter = new ChampionshipMemberSearchFilter()
            {
                Championship = new Championship () {  Id = id }
            };
            var data = _championshipMemberManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<ChampionshipMemberResponse>>(data);

            return Ok(response);
        }

        [HttpGet("{id}/members/pending")]
        public ActionResult<PaginatedResultResponse<ChampionshipMemberInvitationResponse>> GetPendingInvitations([FromRoute] int id, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var data = _championshipMemberManager.GetMemberInvitations(id, isPending: true, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<ChampionshipMemberInvitationResponse>>(data);

            return Ok(response);
        }

        [HttpPost("members")]
        public ActionResult AddMemberInvitation([FromBody] ChampionshipMemberInvitationRequest championshipMemberInvitationRequest)
        {
            var data = _mapper.Map<ChampionshipMemberInvitation>(championshipMemberInvitationRequest);

            _championshipMemberManager.CreateInvitation(data);

            return Ok();
        }

        [HttpPut("members")]
        public ActionResult UpdateMemberInvitation([FromBody] ChampionshipMemberInvitationRequest championshipMemberInvitationRequest)
        {
            var data = _mapper.Map<ChampionshipMemberInvitation>(championshipMemberInvitationRequest);

            _championshipMemberManager.UpdateInvitation(data);

            return Ok();
        }

        [HttpDelete("members/{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _championshipMemberManager.Remove(id);

            return Ok();
        }

        [HttpDelete("members/pending/{id}")]
        public ActionResult DeleteMemberInvitation([FromRoute] int id)
        {
            _championshipMemberManager.RemoveInvitation(id);

            return Ok();
        }
    }
}