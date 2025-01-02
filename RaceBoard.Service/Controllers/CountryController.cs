using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.Country.Request;
using RaceBoard.DTOs.Country.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/countries")]
    [ApiController]
    public class CountryController : AbstractController<CountryController>
    {
        private readonly ICountryManager _countryManager;

        public CountryController
            (
                IMapper mapper,
                ILogger<CountryController> logger,
                ITranslator translator,
                ICountryManager countryManager,
                ISessionHelper sessionHelper,
                IRequestContextManager requestContextManager
            ) : base(mapper, logger, translator, sessionHelper, requestContextManager)
        {
            _countryManager = countryManager;
        }


        [HttpGet()]
        public ActionResult<PaginatedResultResponse<CountryResponse>> Get([FromQuery] CountrySearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<CountrySearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var data = _countryManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<CountryResponse>>(data);

            return Ok(response);
        }

        #region Private Methods

        #endregion
    }
}
