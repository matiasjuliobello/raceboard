using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.File.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/file-types")]
    [ApiController]
    public class FileTypeController : AbstractController<FileTypeController>
    {
        private readonly IFileTypeManager _fileTypeManager;

        public FileTypeController
            (
                IMapper mapper,
                ILogger<FileTypeController> logger,
                ITranslator translator,
                IFileTypeManager fileTypeManager,
                ISessionHelper sessionHelper,
                IRequestContextManager requestContextManager
            ) : base(mapper, logger, translator, sessionHelper, requestContextManager)
        {
            _fileTypeManager = fileTypeManager;
        }

        [HttpGet()]
        public ActionResult<List<FileTypeResponse>> Get([FromQuery] PaginationFilterRequest? paginationFilterRequest = null, [FromQuery] SortingRequest? sortingRequest = null)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationFilterRequest);
            var sorting = _mapper.Map<Sorting>(sortingRequest);

            var data = _fileTypeManager.Get(paginationFilter, sorting);

            var response = _mapper.Map<PaginatedResultResponse<FileTypeResponse>>(data);

            return Ok(response);
        }

        #region Private Methods

        #endregion
    }
}
