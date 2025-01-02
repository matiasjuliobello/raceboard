using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.Organization.Request;
using RaceBoard.DTOs.Organization.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/organizations")]
    [ApiController]
    public class OrganizationMemberController : AbstractController<OrganizationMemberController>
    {
        private readonly IOrganizationMemberManager _organizationMemberManager;

        public OrganizationMemberController
            (
                IMapper mapper,
                ILogger<OrganizationMemberController> logger,
                ITranslator translator,
                IOrganizationMemberManager organizationMemberManager,
                ISessionHelper sessionHelper,
                IRequestContextManager requestContextManager
            ) : base(mapper, logger, translator, sessionHelper, requestContextManager)
        {
            _organizationMemberManager = organizationMemberManager;
        }

        [HttpGet("{id}/members")]
        public ActionResult<PaginatedResultResponse<OrganizationMemberResponse>> Get([FromRoute] int id, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var searchFilter = new OrganizationMemberSearchFilter()
            {
                Organization = new Organization() {  Id = id }
            };
            var data = _organizationMemberManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<OrganizationMemberResponse>>(data);

            return Ok(response);
        }

        [HttpGet("{id}/members/pending")]
        public ActionResult<PaginatedResultResponse<OrganizationMemberInvitationResponse>> GetPendingInvitations([FromRoute] int id, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var data = _organizationMemberManager.GetMemberInvitations(id, isPending: true, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<OrganizationMemberInvitationResponse>>(data);

            return Ok(response);
        }

        [HttpPost("members")]
        public ActionResult AddMemberInvitation([FromBody] OrganizationMemberInvitationRequest organizationMemberInvitationRequest)
        {
            var data = _mapper.Map<OrganizationMemberInvitation>(organizationMemberInvitationRequest);

            data.RequestUser = base.GetUserFromRequestContext();

            _organizationMemberManager.AddInvitation(data);

            return Ok();
        }

        [HttpPut("members")]
        public ActionResult UpdateMemberInvitation([FromBody] OrganizationMemberInvitationRequest organizationMemberInvitationRequest)
        {
            var data = _mapper.Map<OrganizationMemberInvitation>(organizationMemberInvitationRequest);

            data.User = base.GetUserFromRequestContext();

            _organizationMemberManager.UpdateInvitation(data);

            return Ok();
        }

        [HttpDelete("members/{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _organizationMemberManager.Remove(id);

            return Ok();
        }

        [HttpDelete("members/pending/{id}")]
        public ActionResult DeleteMemberInvitation([FromRoute] int id)
        {
            _organizationMemberManager.RemoveInvitation(id);

            return Ok();
        }
    }
}