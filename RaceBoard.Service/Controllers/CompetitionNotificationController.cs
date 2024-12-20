﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
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
    public class CompetitionNotificationController : AbstractController<CompetitionNotificationController>
    {
        private readonly ICompetitionNotificationManager _competitionNotificationManager;
        private readonly INotificationManager _notificationManager;

        public CompetitionNotificationController
            (
                IMapper mapper,
                ILogger<CompetitionNotificationController> logger,
                ITranslator translator,
                ICompetitionNotificationManager competitionNotificationManager,                
                INotificationManager notificationManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _competitionNotificationManager = competitionNotificationManager;
            _notificationManager = notificationManager;
        }

        [HttpGet("{id}/notifications")]
        public ActionResult<PaginatedResultResponse<CompetitionNotificationResponse>> Get([FromRoute] int id, [FromQuery] CompetitionNotificationSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<CompetitionNotificationSearchFilterRequest, CompetitionNotificationSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            searchFilter.Competition = new Competition() { Id = id };

            var data = _competitionNotificationManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<CompetitionNotificationResponse>>(data);

            return Ok(response);
        }

        [HttpPost("notifications")]
        public async Task<ActionResult<int>> Create(CompetitionNotificationRequest competitionNotificationRequest)
        {
            var competitionNotification = _mapper.Map<CompetitionNotification>(competitionNotificationRequest);

            competitionNotification.CreationUser = base.GetUserFromRequestContext();

            _competitionNotificationManager.Create(competitionNotification);

            await _notificationManager.SendNotifications
                (
                    competitionNotification.Title, 
                    competitionNotification.Message, 
                    competitionNotification.Id, 
                    competitionNotification.RaceClasses.Select(x => x.Id).ToArray()
                );

            return Ok(competitionNotification.Id);
        }

        [HttpDelete("notifications/{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _competitionNotificationManager.Delete(id);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}

