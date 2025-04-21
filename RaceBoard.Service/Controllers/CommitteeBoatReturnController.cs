using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.Championship.Response;
using RaceBoard.DTOs.CommitteeBoatReturn.Request;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/championships")]
    [ApiController]
    public class CommitteeBoatReturnController : AbstractController<CommitteeBoatReturnController>
    {
        private readonly IChampionshipManager _championshipManager;

        public CommitteeBoatReturnController
            (
                IMapper mapper,
                ILogger<CommitteeBoatReturnController> logger,
                ITranslator translator,
                IChampionshipManager championshipManager,
                ISessionHelper sessionHelper,
                IRequestContextManager requestContextManager
            ) : base(mapper, logger, translator, sessionHelper, requestContextManager)
        {
            _championshipManager = championshipManager;
        }

        [HttpGet("{id}/committee-boat-returns")]
        public ActionResult<List<ChampionshipBoatReturnResponse>> Get([FromRoute] int id, [FromQuery] CommitteeBoatReturnSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<CommitteeBoatReturnSearchFilterRequest, CommitteeBoatReturnSearchFilter>(searchFilterRequest);
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
        public ActionResult<int> Create(CommitteeBoatReturnRequest request)
        {
            var raceCommitteeBoatReturn = _mapper.Map<CommitteeBoatReturn>(request);

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
