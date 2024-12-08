using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.Competition.Request;
using RaceBoard.DTOs.Competition.Response;
using RaceBoard.Messaging.Entities;
using RaceBoard.Messaging.Interfaces;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;
using RestSharp;

namespace RaceBoard.Service.Controllers
{
    [Route("api/competitions")]
    [ApiController]
    public class CompetitionNotificationController : AbstractController<CompetitionNotificationController>
    {
        private readonly ICompetitionNotificationManager _competitionNotificationManager;
        private readonly INotificationProvider _notificationProvider;

        public CompetitionNotificationController
            (
                IMapper mapper,
                ILogger<CompetitionNotificationController> logger,
                ITranslator translator,
                ICompetitionNotificationManager competitionNotificationManager,
                INotificationProvider notificationProvider,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _competitionNotificationManager = competitionNotificationManager;
            _notificationProvider = notificationProvider;
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
        public ActionResult<int> Create(CompetitionNotificationRequest competitionNotificationRequest)
        {
            var competitionNotification = _mapper.Map<CompetitionNotification>(competitionNotificationRequest);

            competitionNotification.CreationUser = base.GetUserFromRequestContext();

            _competitionNotificationManager.Create(competitionNotification);

            try
            {
                this.SendNotifications(competitionNotification);
            }
            catch (Exception)
            {
            }

            return Ok(competitionNotification.Id);
        }

        [HttpDelete("notifications/{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _competitionNotificationManager.Delete(id);

            return Ok();
        }

        #region Private Methods

        private void SendNotifications(CompetitionNotification competitionNotification)
        {
            List<Task<RestResponse>> tasks = new List<Task<RestResponse>>();

            Parallel.ForEach(competitionNotification.RaceClasses, raceClass =>
            {
                string topicName = $"{competitionNotification.Competition.Id}_{raceClass.Id}";

                var notification = new Notification()
                {
                    NotificationType = Messaging.Providers.NotificationType.Topic,
                    IdTarget = topicName,
                    Title = competitionNotification.Title,
                    Message = competitionNotification.Message,
                    ImageFileUrl = null
                };

                Task<RestResponse> response = _notificationProvider.SendNotification(notification);

                tasks.Add(response);
            });

            Task.WaitAll(tasks.ToArray());
        }

        #endregion
    }
}

