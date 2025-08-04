using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.Coach.Request;
using RaceBoard.DTOs.Coach.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/coaches")]
    [ApiController]
    public class CoachController : AbstractController<CoachController>
    {
        private readonly ICoachManager _coachManager;

        public CoachController
            (
                IMapper mapper,
                ILogger<CoachController> logger,
                ITranslator translator,
                ICoachManager coachManager,
                ISessionHelper sessionHelper,
                IRequestContextManager requestContextManager
            ) : base(mapper, logger, translator, sessionHelper, requestContextManager)
        {
            _coachManager = coachManager;
        }

        [HttpGet()]
        public ActionResult<PaginatedResultResponse<CoachResponse>> Get([FromQuery] CoachSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<CoachSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var coaches = _coachManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<CoachResponse>>(coaches);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public ActionResult<CoachResponse> Get([FromRoute] int id)
        {
            var data = _coachManager.Get(id);

            var response = _mapper.Map<CoachResponse>(data);

            return Ok(response);
        }

        [HttpPost()]
        public ActionResult<int> Create(CoachRequest coachRequest)
        {
            var coach = _mapper.Map<Coach>(coachRequest);

            _coachManager.Create(coach);

            return Ok(coach.Id);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _coachManager.Delete(id);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}
