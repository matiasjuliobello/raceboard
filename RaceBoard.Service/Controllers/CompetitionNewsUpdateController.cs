using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.Competition.Request;
using RaceBoard.DTOs.Competition.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/competitions")]
    [ApiController]
    public class CompetitionNewsUpdateController : AbstractController<CompetitionNewsUpdateController>
    {
        private readonly ICompetitionNewsUpdateManager _competitionNewsUpdateManagerManager;

        public CompetitionNewsUpdateController
            (
                IMapper mapper,
                ILogger<CompetitionNewsUpdateController> logger,
                ITranslator translator,
                ICompetitionNewsUpdateManager competitionNewsUpdateManagerManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _competitionNewsUpdateManagerManager = competitionNewsUpdateManagerManager;
        }

        [HttpGet("{id}/news-updates")]
        public ActionResult<PaginatedResultResponse<CompetitionNewsUpdateResponse>> Get([FromRoute] int id, [FromQuery] CompetitionNewsUpdateSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<CompetitionNewsUpdateSearchFilterRequest, CompetitionNewsUpdateSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            searchFilter.Competition = new Competition() { Id = id };

            var data = _competitionNewsUpdateManagerManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<CompetitionNewsUpdateResponse>>(data);

            return Ok(response);
        }

        [HttpPost("news-updates")]
        public ActionResult<int> Create(CompetitionNewsUpdateRequest competitionNewsUpdateRequest)
        {
            var data = _mapper.Map<CompetitionNewsUpdate>(competitionNewsUpdateRequest);

            _competitionNewsUpdateManagerManager.Create(data);

            return Ok(data.Id);
        }

        #region Private Methods

        #endregion
    }
}

