using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Exceptions;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.Competition.Request;
using RaceBoard.DTOs.Competition.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/competitions")]
    [ApiController]
    public class CompetitionFlagController : AbstractController<CompetitionFlagController>
    {
        private readonly ICompetitionFlagManager _competitionFlagManager;
        private readonly ICompetitionManager _competitionManager;
        private readonly IFlagManager _flagManager;
        private readonly INotificationManager _notificationManager;
        private readonly IDateTimeHelper _dateTimeHelper;

        public CompetitionFlagController
            (
                IMapper mapper,
                ILogger<CompetitionFlagController> logger,
                ITranslator translator,
                ICompetitionFlagManager competitionFlagManager,
                ICompetitionManager competitionManager,
                IFlagManager flagManager,
                INotificationManager notificationManager,
                IDateTimeHelper dateTimeHelper,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _competitionFlagManager = competitionFlagManager;
            _competitionManager = competitionManager;
            _flagManager = flagManager;
            _notificationManager = notificationManager;
            _dateTimeHelper = dateTimeHelper;
        }

        [HttpGet("{id}/flags")]
        public ActionResult<PaginatedResultResponse<CompetitionFlagGroupResponse>> GetCompetitionFlags([FromRoute] int id, [FromQuery] CompetitionFlagSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<CompetitionFlagSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            if (searchFilter == null)
                throw new FunctionalException(Common.Enums.ErrorType.ValidationError, "SearchFilterIsRequired");

            searchFilter.Competition = new Competition() {  Id = id };

            var competitionFlagGroups = _competitionFlagManager.GetFlags(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<CompetitionFlagGroupResponse>>(competitionFlagGroups);

            return Ok(response);
        }

        [HttpPost("flags")]
        public ActionResult<int> RaiseFlags([FromBody] CompetitionFlagGroupRequest competitionFlagGroupRequest)
        {
            var competitionFlagGroup = _mapper.Map<CompetitionFlagGroup>(competitionFlagGroupRequest);

            var currentTime = _dateTimeHelper.GetCurrentTimestamp();
            var currentUser = base.GetUserFromRequestContext();

            int? hoursToLower = competitionFlagGroupRequest.Flags[0].HoursToLower;
            int? minuteToLower = competitionFlagGroupRequest.Flags[0].MinutesToLower;

            foreach (var competitionFlag in competitionFlagGroup.Flags)
            {
                competitionFlag.Person = new Person() { User = new User() { Id = currentUser.Id } };
                competitionFlag.Raising = currentTime;

                if (hoursToLower != null)
                {
                    if (competitionFlag.Lowering == null)
                        competitionFlag.Lowering = competitionFlag.Raising;

                    competitionFlag.Lowering = competitionFlag.Lowering.Value.AddHours(hoursToLower.Value);
                }
                if (minuteToLower != null)
                {
                    if (competitionFlag.Lowering == null)
                        competitionFlag.Lowering = competitionFlag.Raising;

                    competitionFlag.Lowering = competitionFlag.Lowering.Value.AddMinutes(minuteToLower.Value);
                }
            }

            _competitionFlagManager.RaiseFlags(competitionFlagGroup);

            #region Notifications
            Flag[] flags = _flagManager.Get(new FlagSearchFilter() { Ids = competitionFlagGroup.Flags.Select(x => x.Id).ToArray() }).Results.ToArray();
            string listOfFlagNames = String.Join(", ", flags.Select(x => x.Name));

            var competitionGroups = _competitionManager.GetGroups(competitionFlagGroup.Competition.Id);
            int[] idsRaceClasses = competitionGroups.SelectMany(x => x.RaceClasses.Select(y => y.Id)).ToArray();

            _notificationManager.SendNotifications
                (
                    base.Translate("NewFlagsHoisted"),
                    listOfFlagNames,
                    competitionFlagGroup.Competition.Id,
                    idsRaceClasses
                );
            #endregion

            return Ok(competitionFlagGroup.Id);
        }

        [HttpPut("flags")]
        public ActionResult LowerFlags([FromBody] CompetitionFlagGroupRequest competitionFlagGroupRequest)
        {
            var competitionFlagGroup = _mapper.Map<CompetitionFlagGroup>(competitionFlagGroupRequest);

            var currentTime = _dateTimeHelper.GetCurrentTimestamp();
            var currentUser = base.GetUserFromRequestContext();

            foreach (var competitionFlag in competitionFlagGroup.Flags)
            {
                competitionFlag.Person = new Person() { User = new User() { Id = currentUser.Id } };
                competitionFlag.Lowering = currentTime;
            }

            _competitionFlagManager.LowerFlags(competitionFlagGroup);

            return Ok();
        }

        [HttpDelete("flags/{idCompetitionFlagGroup}")]
        public ActionResult DeleteFlags([FromRoute] int idCompetitionFlagGroup)
        {
            _competitionFlagManager.RemoveFlags(idCompetitionFlagGroup);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}

