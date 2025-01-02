using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.Boat.Response;
using RaceBoard.DTOs.Flag.Request;
using RaceBoard.DTOs.Flag.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/flags")]
    [ApiController]
    public class FlagController : AbstractController<FlagController>
    {
        private readonly IFlagManager _flagManager;

        public FlagController
            (
                IMapper mapper,
                ILogger<FlagController> logger,
                ITranslator translator,
                IFlagManager flagManager,
                ISessionHelper sessionHelper,
                IRequestContextManager requestContextManager
            ) : base(mapper, logger, translator, sessionHelper, requestContextManager)
        {
            _flagManager = flagManager;
        }

        [HttpGet()]
        public ActionResult<PaginatedResultResponse<FlagResponse>> Get([FromQuery] FlagSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<FlagSearchFilterRequest, FlagSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var data = _flagManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<FlagResponse>>(data);

            return Ok(response);
        }

        #region Private Methods

        #endregion
    }
}
