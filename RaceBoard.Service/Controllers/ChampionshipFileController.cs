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
    public class ChampionshipFileController : AbstractController<ChampionshipFileController>
    {
        private readonly IChampionshipFileManager _championshipFileManager;
        private readonly INotificationManager _notificationManager;

        public ChampionshipFileController
            (
                IMapper mapper,
                ILogger<ChampionshipFileController> logger,
                ITranslator translator,
                IChampionshipFileManager championshipFileManager,
                INotificationManager notificationManager,
                ISessionHelper sessionHelper,
                IRequestContextManager requestContextManager
            ) : base(mapper, logger, translator, sessionHelper, requestContextManager)
        {
            _championshipFileManager = championshipFileManager;
            _notificationManager = notificationManager;
        }

        [HttpGet("{id}/files")]
        public ActionResult<PaginatedResultResponse<ChampionshipFileResponse>> Get([FromRoute] int id, [FromQuery] ChampionshipFileSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<ChampionshipFileSearchFilterRequest, ChampionshipFileSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            if (sorting.OrderByClauses.Count == 0)
            {
                sorting.OrderByClauses.Add(new OrderByClause("File.CreationDate", OrderByDirection.Descending));
            }

            searchFilter.Championship = new Championship() { Id = id };

            var data = _championshipFileManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<ChampionshipFileResponse>>(data);

            return Ok(response);
        }
        
        [HttpPost("files")]
        public ActionResult<int> Create(IFormFile file, [FromForm] ChampionshipFileRequest championshipFileUploadRequest)
        {
            var validationResult = ValidateBadRequestMessage(file, championshipFileUploadRequest);
            if (!validationResult.success)
            {
                return ReturnBadRequestResponse(validationResult.errorMessage);
            }

            var uploadedFile = _mapper.Map<IFormFile, FileUpload>(file);
            var championshipFile = _mapper.Map<ChampionshipFile>(championshipFileUploadRequest);

            championshipFile.File = base.CreateFileInstance(uploadedFile);
            championshipFile.File.Description = championshipFileUploadRequest.Description;

            _championshipFileManager.Create(championshipFile);

            #region Notifications
            _notificationManager.SendNotifications
                (
                    base.Translate("NewFileHasBeenUploaded"),
                    championshipFile.File.Description,
                    championshipFile.Championship.Id,
                    championshipFile.RaceClasses.Select(x => x.Id).ToArray()
                );
            #endregion

            return Ok();
        }

        [HttpDelete("files/{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _championshipFileManager.Delete(id);

            return Ok();
        }

        #region Private Methods

        private (bool success, string errorMessage) ValidateBadRequestMessage(IFormFile file, ChampionshipFileRequest championshipFileUploadRequest)
        {
            if (file == null || file.Length == 0)
                return new(false, "NoFileWasSelected");

            if (championshipFileUploadRequest.IdChampionship <= 0)
                return new(false, "IdChampionshipIsMissing");

            return new(true, null);
        }

        #endregion
    }
}

