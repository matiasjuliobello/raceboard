using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.Boat.Request;
using RaceBoard.DTOs.Boat.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/boats")]
    [ApiController]
    public class BoatController : AbstractController<BoatController>
    {
        private readonly IBoatManager _boatManager;

        public BoatController
            (
                IMapper mapper,
                ILogger<BoatController> logger,
                ITranslator translator,
                IBoatManager boatManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _boatManager = boatManager;
        }

        [HttpGet()]
        public ActionResult<PaginatedResultResponse<BoatResponse>> GetBoats([FromQuery] BoatSearchFilterRequest searchFilterRequest, [FromQuery] PaginationFilterRequest paginationFilterRequest, [FromQuery] SortingRequest sortingRequest)
        {
            var searchFilter = _mapper.Map<BoatSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var boats = _boatManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<BoatResponse>>(boats);

            return Ok(response);
        }

        [HttpPost()]
        public ActionResult<int> CreateBoat(BoatRequest boatRequest)
        {
            var boat = _mapper.Map<Boat>(boatRequest);

            _boatManager.Create(boat);

            return Ok(boat.Id);
        }

        [HttpPut()]
        public ActionResult UpdateBoat(BoatRequest boatRequest)
        {
            var boat = _mapper.Map<Boat>(boatRequest);

            _boatManager.Update(boat);

            return Ok();
        }

        [HttpDelete("id")]
        public ActionResult DeleteBoat(int id)
        {
            _boatManager.Delete(id);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}
