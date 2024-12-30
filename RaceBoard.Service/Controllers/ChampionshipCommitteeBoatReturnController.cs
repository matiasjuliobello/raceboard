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
    public class ChampionshipCommitteeBoatReturnController : AbstractController<ChampionshipCommitteeBoatReturnController>
    {
        private readonly IChampionshipManager _championshipManager;

        public ChampionshipCommitteeBoatReturnController
            (
                IMapper mapper,
                ILogger<ChampionshipCommitteeBoatReturnController> logger,
                ITranslator translator,
                IChampionshipManager championshipManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _championshipManager = championshipManager;
        }

        [HttpGet("{id}/committee-boat-returns")]
        public ActionResult<List<ChampionshipBoatReturnResponse>> Get([FromRoute] int id, [FromQuery] ChampionshipBoatReturnSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<ChampionshipBoatReturnSearchFilterRequest, ChampionshipBoatReturnSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            searchFilter.Championship = new Championship() {  Id  = id };

            var committeeBoatReturns = _championshipManager.GetCommitteeBoatReturns(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<ChampionshipBoatReturnResponse>>(committeeBoatReturns);



            return Ok(response);
        }

        //[HttpGet("{idRace}/committee-boat-returns/{id}")]
        //public ActionResult<RaceCommitteeBoatReturnResponse> Get([FromRoute] int idRace, [FromRoute] int id)
        //{
        //    var data = _raceManager.Get(id);

        //    var response = _mapper.Map<RaceCommitteeBoatReturnResponse>(data);

        //    return Ok(response);
        //}

        [HttpPost("committee-boat-returns")]
        public ActionResult<int> Create(ChampionshipBoatReturnRequest request)
        {
            var raceCommitteeBoatReturn = _mapper.Map<ChampionshipBoatReturn>(request);

            _championshipManager.CreateCommitteeBoatReturn(raceCommitteeBoatReturn);

            return Ok(raceCommitteeBoatReturn.Id);
        }

        [HttpDelete("committee-boat-returns/{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _championshipManager.DeleteCommitteeBoatReturn(id);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}
