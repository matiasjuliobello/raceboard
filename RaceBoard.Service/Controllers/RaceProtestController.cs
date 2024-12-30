using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.Race.Request;
using RaceBoard.DTOs.Race.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/championships")]
    [ApiController]
    public class RaceProtestController : AbstractController<RaceProtestController>
    {
        private readonly IRaceManager _raceManager;

        public RaceProtestController
            (
                IMapper mapper,
                ILogger<RaceProtestController> logger,
                ITranslator translator,
                IRaceManager raceManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _raceManager = raceManager;
        }

        [HttpGet("{id}/protests")]
        public ActionResult<List<RaceResponse>> Get([FromQuery] RaceSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<RaceSearchFilterRequest, RaceSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var races = _raceManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<RaceResponse>>(races);

            return Ok(response);
        }

        [HttpGet("protests/{id}")]
        public ActionResult<RaceResponse> Get([FromRoute] int id)
        {
            var data = _raceManager.Get(id);

            var response = _mapper.Map<RaceResponse>(data);

            return Ok(response);
        }

        [HttpPost("protests")]
        public ActionResult<int> Create(RaceProtestRequest raceProtestRequest)
        {
            //var data = _mapper.Map<RaceProtest>(raceProtestRequest);

            //_raceManager.CreateProtest(data);

            //return Ok(data.Id);
            return Ok();
        }

        #region Private Methods

        #endregion
    }
}
