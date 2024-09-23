using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.Team.Request;
using RaceBoard.DTOs.Team.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/teams")]
    [ApiController]
    public class TeamController : AbstractController<TeamController>
    {
        private readonly ITeamManager _teamManager;

        public TeamController
            (
                IMapper mapper,
                ILogger<TeamController> logger,
                ITranslator translator,
                ITeamManager teamManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _teamManager = teamManager;
        }

        [HttpGet()]
        public ActionResult<PaginatedResultResponse<TeamResponse>> Get([FromQuery] TeamSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<TeamSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var data = _teamManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<TeamResponse>>(data);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public ActionResult<TeamResponse> Get([FromRoute] int id)
        {
            var data = _teamManager.Get(id);

            var response = _mapper.Map<TeamResponse>(data);

            return Ok(response);
        }

        [HttpPost()]
        public ActionResult<int> Create(TeamRequest teamRequest)
        {
            var data = _mapper.Map<Team>(teamRequest);

            data.Organization.Id = 1;

            _teamManager.Create(data);

            return Ok(data.Id);
        }

        [HttpPut()]
        public ActionResult Update(TeamRequest teamRequest)
        {
            var data = _mapper.Map<Team>(teamRequest);

            _teamManager.Update(data);

            return Ok();
        }

        [HttpDelete("id")]
        public ActionResult Delete(int id)
        {
            _teamManager.Delete(id);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}
