using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.Permissions.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RoleController : AbstractController<RoleController>
    {
        private readonly IRoleManager _roleManager;

        public RoleController
            (
                IMapper mapper,
                ILogger<RoleController> logger,
                ITranslator translator,
                IRoleManager roleManager,
                ISessionHelper sessionHelper,
                IRequestContextManager requestContextManager
            ) : base(mapper, logger, translator, sessionHelper, requestContextManager)
        {
            _roleManager = roleManager;
        }

        [HttpGet()]
        public ActionResult<List<RoleResponse>> Get([FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var data = _roleManager.Get(paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<RoleResponse>>(data);

            return Ok(response);
        }

        #region Private Methods

        #endregion
    }
}
