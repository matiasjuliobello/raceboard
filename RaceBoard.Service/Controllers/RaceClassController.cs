using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.RaceClass.Request;
using RaceBoard.DTOs.RaceClass.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/race-classes")]
    [ApiController]
    public class RaceClassController : AbstractController<RaceClassController>
    {
        private readonly IRaceClassManager _raceClassManager;

        public RaceClassController
            (
                IMapper mapper,
                ILogger<RaceClassController> logger,
                ITranslator translator,
                IRaceClassManager raceClassManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _raceClassManager = raceClassManager;
        }

        [HttpGet()]
        public ActionResult<List<RaceClassResponse>> Get([FromQuery] RaceClassSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<RaceClassSearchFilterRequest, RaceClassSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var data = _raceClassManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<RaceClassResponse>>(data);

            return Ok(response);
        }

        #region Private Methods

        #endregion
    }
}
