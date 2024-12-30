using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.Championship.Request;
using RaceBoard.DTOs.Championship.Response;
using RaceBoard.Service.Attributes;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/championships")]
    [ApiController]
    public class ChampionshipController : AbstractController<ChampionshipController>
    {
        private readonly IChampionshipManager _championshipManager;

        public ChampionshipController
            (
                IMapper mapper,
                ILogger<ChampionshipController> logger,
                ITranslator translator,
                IChampionshipManager championshipManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _championshipManager = championshipManager;
        }

        [HttpGet()]
        //[Authorize(Action = Enums.Action.Championship_Get)]
        public ActionResult<PaginatedResultResponse<ChampionshipResponse>> Get([FromQuery] ChampionshipSearchFilterRequest? searchFilterRequest = null, [FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var searchFilter = _mapper.Map<ChampionshipSearchFilter>(searchFilterRequest);
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var data = _championshipManager.Get(searchFilter, paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<ChampionshipResponse>>(data);

            return Ok(response);
        }

        [HttpGet("{id}")]
        //[Authorize(Action = Enums.Action.Championship_Get)]
        public ActionResult<ChampionshipResponse> Get([FromRoute] int id)
        {
            var data = _championshipManager.Get(id);

            var response = _mapper.Map<ChampionshipResponse>(data);

            return Ok(response);
        }

        [HttpPost()]
        //[Authorize(Action = Enums.Action.Championship_Create)]
        public ActionResult<int> Create(IFormFile imageFile, [FromForm] ChampionshipRequest championshipRequest)
        {
            var championship = _mapper.Map<Championship>(championshipRequest);

            var uploadedImage = _mapper.Map<IFormFile, FileUpload>(imageFile);
            if (uploadedImage != null)
            {
                championship.ImageFile = base.CreateFileInstance(uploadedImage);
                championship.ImageFile.Description = "";
            }

            championship.CreationUser = base.GetUserFromRequestContext();

            _championshipManager.Create(championship);

            return Ok(championship.Id);
        }

        [HttpPut()]
        //[Authorize(Action = Enums.Action.Championship_Update)]
        public ActionResult Update(IFormFile imageFile, [FromForm] ChampionshipRequest championshipRequest)
        {
            var championship = _mapper.Map<Championship>(championshipRequest);

            var uploadedImage = _mapper.Map<IFormFile, FileUpload>(imageFile);
            if (uploadedImage != null)
            {
                championship.ImageFile = base.CreateFileInstance(uploadedImage);
                championship.ImageFile.Description = "";
            }

            _championshipManager.Update(championship);

            return Ok();
        }

        [HttpDelete("id")]
        //[Authorize(Action = Enums.Action.Championship_Delete)]
        public ActionResult Delete(int id)
        {
            _championshipManager.Delete(id);

            return Ok();
        }

        #region Private Methods

        #endregion
    }
}
