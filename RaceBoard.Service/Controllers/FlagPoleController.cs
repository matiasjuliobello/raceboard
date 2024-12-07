using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Exceptions;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.Mast.Request;
using RaceBoard.DTOs.Mast.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/flag-poles")]
    [ApiController]
    public class FlagPoleController : AbstractController<FlagPoleController>
    {
        private readonly IMastManager _mastManager;
        private readonly IDateTimeHelper _dateTimeHelper;

        public FlagPoleController
            (
                IMapper mapper,
                ILogger<FlagPoleController> logger,
                ITranslator translator,
                IMastManager mastManager,
                ISessionHelper sessionHelper,
                IDateTimeHelper dateTimeHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _mastManager = mastManager;
            _dateTimeHelper = dateTimeHelper;
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
        public ActionResult<PaginatedResultResponse<MastFlagResponse>> GetMastFlags([FromQuery] MastFlagSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<MastFlagSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            if (searchFilter == null)
                throw new FunctionalException(Common.Enums.ErrorType.ValidationError, "SearchFilterIsRequired");

            //if (searchFilter.Mast == null && searchFilter.Mast == null)
            //{
            //    throw new FunctionalException(Common.Enums.ErrorType.ValidationError, "IdMastIsRequired");
            //}
            if (searchFilter.Mast == null && searchFilter.Competition == null)
            {
                throw new FunctionalException(Common.Enums.ErrorType.ValidationError, "IdCompetitionIsRequired");
            }

            var mastFlags = _mastManager.GetFlags(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<MastFlagResponse>>(mastFlags);

            return Ok(response);
        }

        [HttpPost("flags")]
        public ActionResult<int> RaiseMastFlag([FromBody] MastFlagRequest mastFlagRequest)
        {
            var mastFlag = _mapper.Map<MastFlag>(mastFlagRequest);

            var currentUser = base.GetUserFromRequestContext();
            mastFlag.Person = new Person() { User = new User() { Id = currentUser.Id } };

            var currentTime = _dateTimeHelper.GetCurrentTimestamp();
            mastFlag.RaisingMoment = currentTime;

            if (mastFlagRequest.HoursToLower != null)
            {
                if (mastFlag.LoweringMoment == null)
                    mastFlag.LoweringMoment = mastFlag.RaisingMoment;

                mastFlag.LoweringMoment = mastFlag.LoweringMoment.Value.AddHours(mastFlagRequest.HoursToLower.Value);
            }
            if (mastFlagRequest.MinutesToLower != null)
            {
                if (mastFlag.LoweringMoment == null)
                    mastFlag.LoweringMoment = mastFlag.RaisingMoment;

                mastFlag.LoweringMoment = mastFlag.LoweringMoment.Value.AddMinutes(mastFlagRequest.MinutesToLower.Value);
            }

            _mastManager.RaiseFlag(mastFlag);

            return Ok(mastFlag.Id);
        }

        [HttpPut("flags")]
        public ActionResult LowerMastFlag([FromBody] MastFlagRequest mastFlagRequest)
        {
            var mastFlag = _mapper.Map<MastFlag>(mastFlagRequest);

            var currentUser = base.GetUserFromRequestContext();
            mastFlag.Person = new Person() { User = new User() { Id = currentUser.Id } };

            var currentTime = _dateTimeHelper.GetCurrentTimestamp();
            mastFlag.LoweringMoment = currentTime;

            _mastManager.LowerFlag(mastFlag);

            return Ok();
        }

        [HttpDelete("flags/{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _mastManager.RemoveFlag(id);

            return Ok();
        }

        #region Private Methods

        #endregion   
    }
}
