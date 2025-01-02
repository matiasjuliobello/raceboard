using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
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
    public class ChampionshipNotificationController : AbstractController<ChampionshipNotificationController>
    {
        private readonly IChampionshipNotificationManager _championshipNotificationManager;
        private readonly INotificationManager _notificationManager;

        public ChampionshipNotificationController
            (
                IMapper mapper,
                ILogger<ChampionshipNotificationController> logger,
                ITranslator translator,
                IChampionshipNotificationManager championshipNotificationManager,                
                INotificationManager notificationManager,
                ISessionHelper sessionHelper,
                IRequestContextManager requestContextManager
            ) : base(mapper, logger, translator, sessionHelper, requestContextManager)
        {
            _championshipNotificationManager = championshipNotificationManager;
            _notificationManager = notificationManager;
        }

        [HttpGet("{id}/notifications")]
        public ActionResult<PaginatedResultResponse<ChampionshipNotificationResponse>> Get([FromRoute] int id, [FromQuery] ChampionshipNotificationSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<ChampionshipNotificationSearchFilterRequest, ChampionshipNotificationSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            searchFilter.Championship = new Championship() { Id = id };

            var data = _championshipNotificationManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<ChampionshipNotificationResponse>>(data);

            return Ok(response);
        }

        [HttpPost("notifications")]
        public async Task<ActionResult<int>> Create(ChampionshipNotificationRequest championshipNotificationRequest)
        {
            var championshipNotification = _mapper.Map<ChampionshipNotification>(championshipNotificationRequest);

            championshipNotification.CreationUser = base.GetUserFromRequestContext();

            _championshipNotificationManager.Create(championshipNotification);

            await _notificationManager.SendNotifications
                (
                    championshipNotification.Title, 
                    championshipNotification.Message, 
                    championshipNotification.Id, 
                    championshipNotification.RaceClasses.Select(x => x.Id).ToArray()
                );

            return Ok(championshipNotification.Id);
        }

        [HttpDelete("notifications/{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _championshipNotificationManager.Delete(id);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}

