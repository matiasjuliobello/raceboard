using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.Boat.Request;
using RaceBoard.DTOs.Boat.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/boats")]
    [ApiController]
    public class BoatOrganizationController : AbstractController<BoatOrganizationController>
    {
        private readonly IBoatOrganizationManager _boatOrganizationManager;

        public BoatOrganizationController
            (
                IMapper mapper,
                ILogger<BoatOrganizationController> logger,
                ITranslator translator,
                IBoatOrganizationManager boatOrganizationManager,
                ISessionHelper sessionHelper,
                IRequestContextManager requestContextManager
            ) : base(mapper, logger, translator, sessionHelper, requestContextManager)
        {
            _boatOrganizationManager = boatOrganizationManager;
        }

        [HttpGet("{id}/organizations")]
        public ActionResult<PaginatedResultResponse<BoatOrganizationResponse>> Get([FromQuery] BoatOrganizationSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<BoatOrganizationSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var boatOrganizations = _boatOrganizationManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<BoatOrganizationResponse>>(boatOrganizations);

            return Ok(response);
        }

        [HttpGet("organizations/{id}")]
        public ActionResult<BoatOrganizationResponse> Get([FromRoute] int id)
        {
            var data = _boatOrganizationManager.Get(id);

            var response = _mapper.Map<BoatOrganizationResponse>(data);

            return Ok(response);
        }

        [HttpPost("organizations")]
        public ActionResult<int> Create(BoatOrganizationRequest boatOrganizationRequest)
        {
            var boatOrganization = _mapper.Map<BoatOrganization>(boatOrganizationRequest);

            _boatOrganizationManager.Create(boatOrganization);

            return Ok(boatOrganization.Id);
        }

        [HttpPut("organizations")]
        public ActionResult Update(BoatOrganizationRequest boatOrganizationRequest)
        {
            var boatOrganization = _mapper.Map<BoatOrganization>(boatOrganizationRequest);

            _boatOrganizationManager.Update(boatOrganization);

            return Ok();
        }

        [HttpDelete("organizations/{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _boatOrganizationManager.Delete(id);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}
