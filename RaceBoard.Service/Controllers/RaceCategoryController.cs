using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.RaceCategory.Request;
using RaceBoard.DTOs.RaceCategory.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/race-categories")]
    [ApiController]
    public class RaceCategoryController : AbstractController<RaceCategoryController>
    {
        private readonly IRaceCategoryManager _raceCategoryManager;

        public RaceCategoryController
            (
                IMapper mapper,
                ILogger<RaceCategoryController> logger,
                ITranslator translator,
                IRaceCategoryManager raceCategoryManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _raceCategoryManager = raceCategoryManager;
        }

        [HttpGet()]
        public ActionResult<List<RaceCategoryResponse>> GetRaceCategoryes([FromQuery] RaceCategorySearchFilterRequest searchFilterRequest, [FromQuery] PaginationFilterRequest paginationFilterRequest, [FromQuery] SortingRequest sortingRequest)
        {
            var searchFilter = _mapper.Map<RaceCategorySearchFilterRequest, RaceCategorySearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var raceCategories = _raceCategoryManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<RaceCategoryResponse>>(raceCategories);

            return Ok(response);
        }

        #region Private Methods

        #endregion
    }
}
