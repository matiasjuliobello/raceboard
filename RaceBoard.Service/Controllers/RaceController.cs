using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.Race.Request;
using RaceBoard.DTOs.Race.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/races")]
    [ApiController]
    public class RaceController : AbstractController<RaceController>
    {
        private readonly IRaceManager _raceManager;

        public RaceController
            (
                IMapper mapper,
                ILogger<RaceController> logger,
                ITranslator translator,
                IRaceManager raceManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _raceManager = raceManager;
        }

        [HttpGet()]
        public ActionResult<List<RaceResponse>> GetRaces([FromQuery] RaceSearchFilterRequest searchFilterRequest, [FromQuery] PaginationFilterRequest paginationFilterRequest, [FromQuery] SortingRequest sortingRequest)
        {
            var searchFilter = _mapper.Map<RaceSearchFilterRequest, RaceSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var races = _raceManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<RaceResponse>>(races);

            return Ok(response);
        }

        [HttpPost()]
        public ActionResult<int> CreateRace(RaceRequest raceRequest)
        {
            var race = _mapper.Map<Race>(raceRequest);

            _raceManager.Create(race);

            return Ok(race.Id);
        }

        [HttpPut()]
        public ActionResult UpdateRace(RaceRequest raceRequest)
        {
            var race = _mapper.Map<Race>(raceRequest);

            _raceManager.Update(race);

            return Ok();
        }

        [HttpDelete("id")]
        public ActionResult DeleteRace(int id)
        {
            _raceManager.Delete(id);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}
