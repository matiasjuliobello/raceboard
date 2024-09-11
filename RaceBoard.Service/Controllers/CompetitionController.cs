using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.Competition.Request;
using RaceBoard.DTOs.Competition.Response;
using RaceBoard.DTOs.Mast.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/competitions")]
    [ApiController]
    public class CompetitionController : AbstractController<CompetitionController>
    {
        private readonly ICompetitionManager _competitionManager;

        public CompetitionController
            (
                IMapper mapper,
                ILogger<CompetitionController> logger,
                ITranslator translator,
                ICompetitionManager competitionManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _competitionManager = competitionManager;
        }

        [HttpGet()]
        public ActionResult<PaginatedResultResponse<CompetitionResponse>> Get([FromQuery] CompetitionSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<CompetitionSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var data = _competitionManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<CompetitionResponse>>(data);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public ActionResult<CompetitionResponse> Get([FromRoute] int id)
        {
            var data = _competitionManager.Get(id);

            var response = _mapper.Map<CompetitionResponse>(data);

            return Ok(response);
        }

        [HttpPost()]
        public ActionResult<int> Create(CompetitionRequest competitionRequest)
        {
            var competition = _mapper.Map<Competition>(competitionRequest);

            _competitionManager.Create(competition);

            return Ok(competition.Id);
        }

        [HttpPut()]
        public ActionResult Update(CompetitionRequest competitionRequest)
        {
            var competition = _mapper.Map<Competition>(competitionRequest);

            _competitionManager.Update(competition);

            return Ok();
        }

        [HttpDelete("id")]
        public ActionResult Delete(int id)
        {
            _competitionManager.Delete(id);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}
