using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Exceptions;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.Flag.Response;
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
        public ActionResult<List<MastResponse>> Get([FromQuery] MastSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<MastSearchFilterRequest, MastSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var data = _mastManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<MastResponse>>(data);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public ActionResult<MastResponse> Get([FromRoute] int id)
        {
            var data = _mastManager.Get(id);

            var response = _mapper.Map<MastResponse>(data);

            return Ok(response);
        }

        [HttpPost()]
        public ActionResult<int> Create(MastRequest mastRequest)
        {
            var mast = _mapper.Map<Mast>(mastRequest);

            _mastManager.Create(mast);

            return Ok(mast.Id);
        }

        [HttpGet("flags")]
        public ActionResult<List<MastFlagResponse>> GetMastFlags([FromQuery] MastFlagSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<MastFlagSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            if (searchFilter == null || searchFilter.Mast == null)
            {
                throw new FunctionalException(Common.Enums.ErrorType.ValidationError, "IdMastIsRequired");
            }

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
