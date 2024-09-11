using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.BloodType.Request;
using RaceBoard.DTOs.BloodType.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/blood-types")]
    [ApiController]
    public class BloodTypeController : AbstractController<BloodTypeController>
    {
        private readonly IBloodTypeManager _bloodTypeManager;

        public BloodTypeController
            (
                IMapper mapper,
                ILogger<BloodTypeController> logger,
                ITranslator translator,
                IBloodTypeManager bloodTypeManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _bloodTypeManager = bloodTypeManager;
        }

        [HttpGet()]
        public ActionResult<List<BloodTypeResponse>> Get([FromQuery] BloodTypeSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<BloodTypeSearchFilterRequest, BloodTypeSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var data = _bloodTypeManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<BloodTypeResponse>>(data);

            return Ok(response);
        }

        #region Private Methods

        #endregion
    }
}
