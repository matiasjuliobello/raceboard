using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.TeamMemberRole.Request;
using RaceBoard.DTOs.TeamMemberRole.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/team-member-roles")]
    [ApiController]
    public class TeamMemberRoleController : AbstractController<TeamMemberRoleController>
    {
        private readonly ITeamMemberRoleManager _teamMemberRoleManager;

        public TeamMemberRoleController
            (
                IMapper mapper,
                ILogger<TeamMemberRoleController> logger,
                ITranslator translator,
                ITeamMemberRoleManager teamMemberRoleManager,
                ISessionHelper sessionHelper,
                IRequestContextManager requestContextManager
            ) : base(mapper, logger, translator, sessionHelper, requestContextManager)
        {
            _teamMemberRoleManager = teamMemberRoleManager;
        }

        [HttpGet()]
        public ActionResult<PaginatedResultResponse<TeamMemberRoleResponse>> Get([FromQuery] TeamMemberRoleSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<TeamMemberRoleSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var data = _teamMemberRoleManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<TeamMemberRoleResponse>>(data);

            return Ok(response);
        }

        #region Private Methods

        #endregion
    }
}
