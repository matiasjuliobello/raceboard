﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.Flag.Request;
using RaceBoard.DTOs.Flag.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/flags")]
    [ApiController]
    public class FlagController : AbstractController<FlagController>
    {
        private readonly IFlagManager _flagManager;

        public FlagController
            (
                IMapper mapper,
                ILogger<FlagController> logger,
                ITranslator translator,
                IFlagManager flagManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _flagManager = flagManager;
        }

        [HttpGet()]
        public ActionResult<List<FlagResponse>> GetFlags([FromQuery] FlagSearchFilterRequest searchFilterRequest, [FromQuery] PaginationFilterRequest paginationFilterRequest, [FromQuery] SortingRequest sortingRequest)
        {
            var searchFilter = _mapper.Map<FlagSearchFilterRequest, FlagSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var flags = _flagManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<FlagResponse>>(flags);

            return Ok(response);
        }

        #region Private Methods

        #endregion
    }
}
