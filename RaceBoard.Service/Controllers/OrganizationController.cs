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
using RaceBoard.Service.Helpers;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/organizations")]
    [ApiController]
    public class OrganizationController : AbstractController<OrganizationController>
    {
        private readonly IOrganizationManager _organizationManager;

        public OrganizationController
            (
                IMapper mapper,
                ILogger<OrganizationController> logger,
                ITranslator translator,
                IOrganizationManager organizationManager,
                ISessionHelper sessionHelper,
                IRequestContextManager requestContextManager
            ) : base(mapper, logger, translator, sessionHelper, requestContextManager)
        {
            _organizationManager = organizationManager;
        }

        [HttpGet()]
        public ActionResult<List<OrganizationResponse>> Get([FromQuery] OrganizationSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<OrganizationSearchFilterRequest, OrganizationSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var data = _organizationManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<OrganizationResponse>>(data);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public ActionResult<OrganizationResponse> Get([FromRoute] int id)
        {
            var data = _organizationManager.Get(id);

            var response = _mapper.Map<OrganizationResponse>(data);

            return Ok(response);
        }

        [HttpPost()]
        public ActionResult<int> Create(OrganizationRequest organizationRequest)
        {
            var organization = _mapper.Map<Organization>(organizationRequest);

            organization.CreationUser = base.GetUserFromRequestContext();

            _organizationManager.Create(organization);

            return Ok(organization.Id);
        }

        [HttpPut()]
        public ActionResult Update(OrganizationRequest organizationRequest)
        {
            var organization = _mapper.Map<Organization>(organizationRequest);

            _organizationManager.Update(organization);

            return Ok();
        }

        [HttpDelete("id")]
        public ActionResult Delete(int id)
        {
            _organizationManager.Delete(id);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}
