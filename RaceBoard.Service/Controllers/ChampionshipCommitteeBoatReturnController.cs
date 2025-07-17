using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.Championship.Request;
using RaceBoard.DTOs.Championship.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/championships")]
    [ApiController]
    public class CommitteeBoatReturnController : AbstractController<CommitteeBoatReturnController>
    {
        private readonly IChampionshipCommitteeBoatReturnManager _championshipCommitteeBoatReturnManager;

        public CommitteeBoatReturnController
            (
                IMapper mapper,
                ILogger<CommitteeBoatReturnController> logger,
                ITranslator translator,
                IChampionshipCommitteeBoatReturnManager championshipCommitteeBoatReturnManager,
                ISessionHelper sessionHelper,
                IRequestContextManager requestContextManager
            ) : base(mapper, logger, translator, sessionHelper, requestContextManager)
        {
            _championshipCommitteeBoatReturnManager = championshipCommitteeBoatReturnManager;
        }

        [HttpGet("{id}/committee-boat-returns")]
        public ActionResult<List<ChampionshipCommitteeBoatReturnResponse>> Get([FromRoute] int id, [FromQuery] ChampionshipCommitteeBoatReturnSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<ChampionshipCommitteeBoatReturnSearchFilterRequest, ChampionshipCommitteeBoatReturnSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            searchFilter.Championship = new Championship() {  Id  = id };

            var committeeBoatReturns = _championshipCommitteeBoatReturnManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<ChampionshipCommitteeBoatReturnResponse>>(committeeBoatReturns);


            return Ok(response);
        }

        [HttpGet("committee-boat-returns/{id}")]
        public ActionResult<ChampionshipCommitteeBoatReturnResponse> Get([FromRoute] int id)
        {
            var data = _championshipCommitteeBoatReturnManager.Get(id);

            var response = _mapper.Map<ChampionshipCommitteeBoatReturnResponse>(data);

            return Ok(response);
        }

        [HttpPost("committee-boat-returns")]
        public ActionResult<int> Create(ChampionshipCommitteeBoatReturnRequest request)
        {
            var raceCommitteeBoatReturn = _mapper.Map<ChampionshipCommitteeBoatReturn>(request);

            _championshipCommitteeBoatReturnManager.Create(raceCommitteeBoatReturn);

            return Ok(raceCommitteeBoatReturn.Id);
        }

        [HttpPut("committee-boat-returns")]
        public ActionResult<int> Update(ChampionshipCommitteeBoatReturnRequest request)
        {
            var raceCommitteeBoatReturn = _mapper.Map<ChampionshipCommitteeBoatReturn>(request);

            _championshipCommitteeBoatReturnManager.Update(raceCommitteeBoatReturn);

            return Ok(raceCommitteeBoatReturn.Id);
        }

        [HttpDelete("committee-boat-returns/{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _championshipCommitteeBoatReturnManager.Delete(id);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}
