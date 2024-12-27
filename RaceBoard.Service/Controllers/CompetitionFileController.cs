using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.Competition.Request;
using RaceBoard.DTOs.Competition.Response;
using RaceBoard.Service.Attributes;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/competitions")]
    [ApiController]
    public class CompetitionFileController : AbstractController<CompetitionFileController>
    {
        private readonly ICompetitionFileManager _competitionFileManager;
        private readonly INotificationManager _notificationManager;
        private readonly IDateTimeHelper _dateTimeHelper;

        public CompetitionFileController
            (
                IMapper mapper,
                ILogger<CompetitionFileController> logger,
                ITranslator translator,
                ICompetitionFileManager competitionFileManager,
                INotificationManager notificationManager,
                IDateTimeHelper dateTimeHelper,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _competitionFileManager = competitionFileManager;
            _notificationManager = notificationManager;
            _dateTimeHelper = dateTimeHelper;
        }

        [HttpGet("{id}/files")]
        public ActionResult<PaginatedResultResponse<CompetitionFileResponse>> Get([FromRoute] int id, [FromQuery] CompetitionFileSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<CompetitionFileSearchFilterRequest, CompetitionFileSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            if (sorting.OrderByClauses.Count == 0)
            {
                sorting.OrderByClauses.Add(new OrderByClause("File.CreationDate", OrderByDirection.Descending));
            }

            searchFilter.Competition = new Competition() { Id = id };

            var data = _competitionFileManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<CompetitionFileResponse>>(data);

            return Ok(response);
        }

        [HttpPost("files")]
        public ActionResult<int> Create(IFormFile file, [FromForm] CompetitionFileRequest competitionFileUploadRequest)
        {
            var validationResult = ValidateBadRequestMessage(file, competitionFileUploadRequest);
            if (!validationResult.success)
            {
                return ReturnBadRequestResponse(validationResult.errorMessage);
            }

            var uploadedFile = _mapper.Map<IFormFile, FileUpload>(file);
            var competitionFile = _mapper.Map<CompetitionFile>(competitionFileUploadRequest);

            competitionFile.File = base.CreateFileInstance(uploadedFile);
            competitionFile.File.Description = competitionFileUploadRequest.Description;

            _competitionFileManager.Create(competitionFile);

            #region Notifications
            _notificationManager.SendNotifications
                (
                    base.Translate("NewFileHasBeenUploaded"),
                    competitionFile.File.Description,
                    competitionFile.Competition.Id,
                    competitionFile.RaceClasses.Select(x => x.Id).ToArray()
                );
            #endregion

            return Ok();
        }

        [HttpDelete("files/{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _competitionFileManager.Delete(id);

            return Ok();
        }

        #region Private Methods

        private (bool success, string errorMessage) ValidateBadRequestMessage(IFormFile file, CompetitionFileRequest competitionFileUploadRequest)
        {
            if (file == null || file.Length == 0)
                return new(false, "NoFileWasSelected");

            if (competitionFileUploadRequest.IdCompetition <= 0)
                return new(false, "IdCompetitionIsMissing");

            return new(true, null);
        }

        #endregion
    }
}

