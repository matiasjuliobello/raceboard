﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Exceptions;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.Championship.Request;
using RaceBoard.DTOs.Championship.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/championships")]
    [ApiController]
    public class ChampionshipFlagController : AbstractController<ChampionshipFlagController>
    {
        private readonly IChampionshipFlagManager _championshipFlagManager;
        private readonly IChampionshipManager _championshipManager;
        private readonly IAuthorizationManager _authorizationManager;
        private readonly IFlagManager _flagManager;
        private readonly INotificationManager _notificationManager;
        private readonly IDateTimeHelper _dateTimeHelper;

        public ChampionshipFlagController
            (
                IMapper mapper,
                ILogger<ChampionshipFlagController> logger,
                ITranslator translator,
                IChampionshipFlagManager championshipFlagManager,
                IChampionshipManager championshipManager,
                IAuthorizationManager authorizationManager,
                IFlagManager flagManager,
                INotificationManager notificationManager,
                IDateTimeHelper dateTimeHelper,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _championshipFlagManager = championshipFlagManager;
            _championshipManager = championshipManager;
            _flagManager = flagManager;
            _notificationManager = notificationManager;
            _authorizationManager = authorizationManager;
            _dateTimeHelper = dateTimeHelper;
        }

        [HttpGet("{id}/flags")]
        public ActionResult<PaginatedResultResponse<ChampionshipFlagGroupResponse>> GetChampionshipFlags([FromRoute] int id, [FromQuery] ChampionshipFlagSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<ChampionshipFlagSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            if (searchFilter == null)
                throw new FunctionalException(Common.Enums.ErrorType.ValidationError, "SearchFilterIsRequired");

            searchFilter.Championship = new Championship() {  Id = id };

            var championshipFlagGroups = _championshipFlagManager.GetFlags(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<ChampionshipFlagGroupResponse>>(championshipFlagGroups);

            return Ok(response);
        }

        [HttpPost("flags")]
        public ActionResult<int> RaiseFlags([FromBody] ChampionshipFlagGroupRequest championshipFlagGroupRequest)
        {
            var currentUser = base.GetUserFromRequestContext();
            _authorizationManager.ValidatePermission(Domain.Enums.Action.ChampionshipFlag_Create, championshipFlagGroupRequest.IdChampionship, currentUser.Id);


            var championshipFlagGroup = _mapper.Map<ChampionshipFlagGroup>(championshipFlagGroupRequest);

            var currentTime = _dateTimeHelper.GetCurrentTimestamp();
            //var currentUser = base.GetUserFromRequestContext();

            int? hoursToLower = championshipFlagGroupRequest.Flags[0].HoursToLower;
            int? minuteToLower = championshipFlagGroupRequest.Flags[0].MinutesToLower;

            foreach (var championshipFlag in championshipFlagGroup.Flags)
            {
                championshipFlag.User = new User() { Id = currentUser.Id };
                championshipFlag.Raising = currentTime;

                if (hoursToLower != null)
                {
                    if (championshipFlag.Lowering == null)
                        championshipFlag.Lowering = championshipFlag.Raising;

                    championshipFlag.Lowering = championshipFlag.Lowering.Value.AddHours(hoursToLower.Value);
                }
                if (minuteToLower != null)
                {
                    if (championshipFlag.Lowering == null)
                        championshipFlag.Lowering = championshipFlag.Raising;

                    championshipFlag.Lowering = championshipFlag.Lowering.Value.AddMinutes(minuteToLower.Value);
                }
            }

            _championshipFlagManager.RaiseFlags(championshipFlagGroup);

            #region Notifications
            Flag[] flags = _flagManager.Get(new FlagSearchFilter() { Ids = championshipFlagGroup.Flags.Select(x => x.Id).ToArray() }).Results.ToArray();
            string listOfFlagNames = String.Join(", ", flags.Select(x => x.Name));

            var championshipGroups = _championshipManager.GetGroups(championshipFlagGroup.Championship.Id);
            int[] idsRaceClasses = championshipGroups.SelectMany(x => x.RaceClasses.Select(y => y.Id)).ToArray();

            _notificationManager.SendNotifications
                (
                    base.Translate("NewFlagsHoisted"),
                    listOfFlagNames,
                    championshipFlagGroup.Championship.Id,
                    idsRaceClasses
                );
            #endregion

            return Ok(championshipFlagGroup.Id);
        }

        [HttpPut("flags")]
        public ActionResult LowerFlags([FromBody] ChampionshipFlagGroupRequest championshipFlagGroupRequest)
        {
            var championshipFlagGroup = _mapper.Map<ChampionshipFlagGroup>(championshipFlagGroupRequest);

            var currentTime = _dateTimeHelper.GetCurrentTimestamp();
            var currentUser = base.GetUserFromRequestContext();

            foreach (var championshipFlag in championshipFlagGroup.Flags)
            {
                championshipFlag.User = new User() { Id = currentUser.Id };
                championshipFlag.Lowering = currentTime;
            }

            _championshipFlagManager.LowerFlags(championshipFlagGroup);

            return Ok();
        }

        //[HttpDelete("flags/{idChampionshipFlagGroup}")]
        //public ActionResult DeleteFlags([FromRoute] int idChampionshipFlagGroup)
        //{
        //    _championshipFlagManager.RemoveFlags(idChampionshipFlagGroup);

        //    return Ok();
        //}

        #region Private Methods

        #endregion
    }
}
