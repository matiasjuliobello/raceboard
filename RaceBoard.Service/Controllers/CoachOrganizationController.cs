using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.Coach.Request;
using RaceBoard.DTOs.Coach.Response;
using RaceBoard.DTOs.Team.Request;
using RaceBoard.DTOs.Team.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/coaches")]
    [ApiController]
    public class CoachOrganizationController : AbstractController<CoachOrganizationController>
    {
        private readonly ICoachOrganizationManager _coachOrganizationManager;

        public CoachOrganizationController
            (
                IMapper mapper,
                ILogger<CoachOrganizationController> logger,
                ITranslator translator,
                ICoachOrganizationManager coachOrganizationManager,
                ISessionHelper sessionHelper,
                IRequestContextManager requestContextManager
            ) : base(mapper, logger, translator, sessionHelper, requestContextManager)
        {
            _coachOrganizationManager = coachOrganizationManager;
        }

        [HttpGet("{id}/organizations")]
        public ActionResult<PaginatedResultResponse<CoachOrganizationResponse>> Get([FromRoute] int id, [FromQuery] CoachOrganizationSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<CoachOrganizationSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            searchFilter.Coach = new Coach() { Id = id };

            var coachOrganizations = _coachOrganizationManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<CoachOrganizationResponse>>(coachOrganizations);

            return Ok(response);
        }

        [HttpGet("organizations/{id}")]
        public ActionResult<CoachOrganizationResponse> Get([FromRoute] int id)
        {
            var data = _coachOrganizationManager.Get(id);

            var response = _mapper.Map<CoachOrganizationResponse>(data);

            return Ok(response);
        }

        [HttpPost("organizations")]
        public ActionResult CreateCoachOrganization([FromBody] CoachOrganizationRequest coachOrganizationRequest)
        {
            var data = _mapper.Map<CoachOrganization>(coachOrganizationRequest);

            _coachOrganizationManager.Create(data);

            return Ok();
        }

        [HttpPut("organizations")]
        public ActionResult UpdateCoachOrganization([FromBody] CoachOrganizationRequest coachOrganizationRequest)
        {
            var data = _mapper.Map<CoachOrganization>(coachOrganizationRequest);

            _coachOrganizationManager.Update(data);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}
