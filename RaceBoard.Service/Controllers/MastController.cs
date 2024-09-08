﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.Mast.Request;
using RaceBoard.DTOs.Mast.Request;
using RaceBoard.DTOs.Mast.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/masts")]
    [ApiController]
    public class MastController : AbstractController<MastController>
    {
        private readonly IMastManager _mastManager;

        public MastController
            (
                IMapper mapper,
                ILogger<MastController> logger,
                ITranslator translator,
                IMastManager mastManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _mastManager = mastManager;
        }

        [HttpGet()]
        public ActionResult<List<MastResponse>> GetMasts([FromQuery] MastSearchFilterRequest searchFilterRequest, [FromQuery] PaginationFilterRequest paginationFilterRequest, [FromQuery] SortingRequest sortingRequest)
        {
            var searchFilter = _mapper.Map<MastSearchFilterRequest, MastSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var masts = _mastManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<MastResponse>>(masts);

            return Ok(response);
        }

        [HttpPost()]
        public ActionResult<int> CreateMast(MastRequest mastRequest)
        {
            var mast = _mapper.Map<Mast>(mastRequest);

            _mastManager.Create(mast);

            return Ok(mast.Id);
        }

        [HttpGet("flags")]
        public ActionResult<List<MastFlagResponse>> GetMastFlags([FromQuery] MastFlagSearchFilterRequest searchFilterRequest, [FromQuery] PaginationFilterRequest paginationFilterRequest, [FromQuery] SortingRequest sortingRequest)
        {
            var searchFilter = _mapper.Map<MastFlagSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var mastFlags = _mastManager.GetFlags(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<MastFlagResponse>>(mastFlags);

            return Ok(response);
        }

        [HttpPost("flags")]
        public ActionResult<int> RaiseMastFlag([FromBody] MastFlagRequest mastFlagRequest)
        {
            var mastFlag = _mapper.Map<MastFlag>(mastFlagRequest);

            _mastManager.RaiseFlag(mastFlag);

            return Ok(mastFlag.Id);
        }

        [HttpPut("flags")]
        public ActionResult LowerMastFlag([FromBody] MastFlagRequest mastFlagRequest)
        {
            var mastFlag = _mapper.Map<MastFlag>(mastFlagRequest);

            _mastManager.LowerFlag(mastFlag);

            return Ok();
        }

        #region Private Methods

        #endregion   
    }
}