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
    public class BoatOwnerController : AbstractController<BoatOwnerController>
    {
        private readonly IBoatOwnerManager _boatOwnerManager;

        public BoatOwnerController
            (
                IMapper mapper,
                ILogger<BoatOwnerController> logger,
                ITranslator translator,
                IBoatOwnerManager boatOwnerManager,
                ISessionHelper sessionHelper,
                IRequestContextManager requestContextManager
            ) : base(mapper, logger, translator, sessionHelper, requestContextManager)
        {
            _boatOwnerManager = boatOwnerManager;
        }

        [HttpGet("{id}/owners")]
        public ActionResult<PaginatedResultResponse<BoatOwnerResponse>> Get([FromRoute] int id, [FromQuery] BoatOwnerSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            if (searchFilterRequest == null)
                searchFilterRequest = new BoatOwnerSearchFilterRequest();

            searchFilterRequest.IdBoat = id;

            var searchFilter = _mapper.Map<BoatOwnerSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var boatOwners = _boatOwnerManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<BoatOwnerResponse>>(boatOwners);

            return Ok(response);
        }

        [HttpPost("owners")]
        public ActionResult Set(BoatOwnerRequest[] boatOwnerRequests)
        {
            var boatOwners = _mapper.Map<List<BoatOwner>>(boatOwnerRequests);

            _boatOwnerManager.Set(boatOwners);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}
