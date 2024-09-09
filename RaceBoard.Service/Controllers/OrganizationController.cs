using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.Organization.Request;
using RaceBoard.DTOs.Organization.Response;
using RaceBoard.DTOs.Organization.Request;
using RaceBoard.Service.Controllers.Abstract;
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
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _organizationManager = organizationManager;
        }

        [HttpGet()]
        public ActionResult<List<OrganizationResponse>> GetOrganizations([FromQuery] OrganizationSearchFilterRequest searchFilterRequest, [FromQuery] PaginationFilterRequest paginationFilterRequest, [FromQuery] SortingRequest sortingRequest)
        {
            var searchFilter = _mapper.Map<OrganizationSearchFilterRequest, OrganizationSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var organizations = _organizationManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<OrganizationResponse>>(organizations);

            return Ok(response);
        }

        [HttpPost()]
        public ActionResult<int> CreateOrganization(OrganizationRequest organizationRequest)
        {
            var organization = _mapper.Map<Organization>(organizationRequest);

            _organizationManager.Create(organization);

            return Ok(organization.Id);
        }

        [HttpPut()]
        public ActionResult UpdateOrganization(OrganizationRequest organizationRequest)
        {
            var organization = _mapper.Map<Organization>(organizationRequest);

            _organizationManager.Update(organization);

            return Ok();
        }

        [HttpDelete("id")]
        public ActionResult DeleteOrganization(int id)
        {
            _organizationManager.Delete(id);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}
