using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.ContestantRole.Request;
using RaceBoard.DTOs.ContestantRole.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/contestant-roles")]
    [ApiController]
    public class ContestantRoleController : AbstractController<ContestantRoleController>
    {
        private readonly IContestantRoleManager _contestantRoleManager;

        public ContestantRoleController
            (
                IMapper mapper,
                ILogger<ContestantRoleController> logger,
                ITranslator translator,
                IContestantRoleManager contestantRoleManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _contestantRoleManager = contestantRoleManager;
        }

        [HttpGet()]
        public ActionResult<PaginatedResultResponse<ContestantRoleResponse>> Get([FromQuery] ContestantRoleSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<ContestantRoleSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var data = _contestantRoleManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<ContestantRoleResponse>>(data);

            return Ok(response);
        }

        #region Private Methods

        #endregion
    }
}
