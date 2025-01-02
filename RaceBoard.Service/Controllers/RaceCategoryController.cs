﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.RaceCategory.Request;
using RaceBoard.DTOs.RaceCategory.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/race-categories")]
    [ApiController]
    public class RaceCategoryController : AbstractController<RaceCategoryController>
    {
        private readonly IRaceCategoryManager _raceCategoryManager;

        public RaceCategoryController
            (
                IMapper mapper,
                ILogger<RaceCategoryController> logger,
                ITranslator translator,
                IRaceCategoryManager raceCategoryManager,
                ISessionHelper sessionHelper,
                IRequestContextManager requestContextManager
            ) : base(mapper, logger, translator, sessionHelper, requestContextManager)
        {
            _raceCategoryManager = raceCategoryManager;
        }

        [HttpGet()]
        public ActionResult<List<RaceCategoryResponse>> Get([FromQuery] RaceCategorySearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<RaceCategorySearchFilterRequest, RaceCategorySearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var data = _raceCategoryManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<RaceCategoryResponse>>(data);

            return Ok(response);
        }

        #region Private Methods

        #endregion
    }
}
