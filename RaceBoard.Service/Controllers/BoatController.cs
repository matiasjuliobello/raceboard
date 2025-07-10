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
                IRequestContextManager requestContextManager
            ) : base(mapper, logger, translator, sessionHelper, requestContextManager)
        {
            _boatManager = boatManager;
        }

        [HttpGet("search")]
        public ActionResult<PaginatedResultResponse<BoatResponse>> Search([FromQuery] string searchTerm, [FromQuery] int idRaceClass, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var data = _boatManager.Search(searchTerm, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<BoatResponse>>(data);

            return Ok(response);
        }

        [HttpGet()]
        public ActionResult<PaginatedResultResponse<BoatResponse>> Get([FromQuery] BoatSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<BoatSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var boats = _boatManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<BoatResponse>>(boats);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public ActionResult<BoatResponse> Get([FromRoute] int id)
        {
            var data = _boatManager.Get(id);

            var response = _mapper.Map<BoatResponse>(data);

            return Ok(response);
        }

        [HttpPost()]
        public ActionResult<int> Create(BoatRequest boatRequest)
        {
            var boat = _mapper.Map<Boat>(boatRequest);

            _boatManager.Create(boat);

            return Ok(boat.Id);
        }

        [HttpPut()]
        public ActionResult Update(BoatRequest boatRequest)
        {
            var boat = _mapper.Map<Boat>(boatRequest);

            _boatManager.Update(boat);

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _boatManager.Delete(id);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}
