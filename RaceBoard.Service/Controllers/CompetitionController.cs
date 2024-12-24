using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
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
using Enums = RaceBoard.Domain.Enums;

namespace RaceBoard.Service.Controllers
{
    [Route("api/competitions")]
    [ApiController]
    public class CompetitionController : AbstractController<CompetitionController>
    {
        private readonly ICompetitionManager _competitionManager;

        public CompetitionController
            (
                IMapper mapper,
                ILogger<CompetitionController> logger,
                ITranslator translator,
                ICompetitionManager competitionManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _competitionManager = competitionManager;
        }

        [HttpGet()]
        //[Authorize(Action = Enums.Action.Competition_Get)]
        public ActionResult<PaginatedResultResponse<CompetitionResponse>> Get([FromQuery] CompetitionSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<CompetitionSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var data = _competitionManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<CompetitionResponse>>(data);

            return Ok(response);
        }

        [HttpGet("{id}")]
        //[Authorize(Action = Enums.Action.Competition_Get)]
        public ActionResult<CompetitionResponse> Get([FromRoute] int id)
        {
            var data = _competitionManager.Get(id);

            var response = _mapper.Map<CompetitionResponse>(data);

            return Ok(response);
        }

        [HttpPost()]
        //[Authorize(Action = Enums.Action.Competition_Create)]
        public ActionResult<int> Create(IFormFile imageFile, [FromForm] CompetitionRequest competitionRequest)
        {
            var competition = _mapper.Map<Competition>(competitionRequest);

            var uploadedImage = _mapper.Map<IFormFile, FileUpload>(imageFile);
            if (uploadedImage != null)
            {
                competition.ImageFile = base.CreateFileInstance(uploadedImage);
                competition.ImageFile.Description = "";
            }

            _competitionManager.Create(competition);

            return Ok(competition.Id);
        }

        [HttpPut()]
        //[Authorize(Action = Enums.Action.Competition_Update)]
        public ActionResult Update(IFormFile imageFile, [FromForm] CompetitionRequest competitionRequest)
        {
            var competition = _mapper.Map<Competition>(competitionRequest);

            var uploadedImage = _mapper.Map<IFormFile, FileUpload>(imageFile);
            if (uploadedImage != null)
            {
                competition.ImageFile = base.CreateFileInstance(uploadedImage);
                competition.ImageFile.Description = "";
            }

            _competitionManager.Update(competition);

            return Ok();
        }

        [HttpDelete("id")]
        //[Authorize(Action = Enums.Action.Competition_Delete)]
        public ActionResult Delete(int id)
        {
            _competitionManager.Delete(id);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}
