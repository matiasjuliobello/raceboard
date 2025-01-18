using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.Common;
using RaceBoard.Common.Helpers.Pagination;
using RaceBoard.Domain;
using RaceBoard.DTOs._Pagination.Request;
using RaceBoard.DTOs._Pagination.Response;
using RaceBoard.DTOs.File.Request;
using RaceBoard.DTOs.File.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FileController : AbstractController<FileController>
    {
        private readonly IFileManager _flagManager;

        public FileController
            (
                IMapper mapper,
                ILogger<FileController> logger,
                ITranslator translator,
                IFileManager flagManager,
                ISessionHelper sessionHelper,
                IRequestContextManager requestContextManager
            ) : base(mapper, logger, translator, sessionHelper, requestContextManager)
        {
            _flagManager = flagManager;
        }

        [HttpGet("{id}")]
        public ActionResult DownloadFile([FromRoute] int id)
        {
            Domain.File file = _flagManager.Get(id);

            string fileName = file.Description;
            string fileFullPath = Path.Combine(file.Path, file.Name);
            var fileStream =  new FileStream(fileFullPath, FileMode.Open);

            return new FileStreamResult(fileStream, CommonValues.MimeTypes.ApplicationOctetStream)
            {
                //FileDownloadName = new FileInfo(((FileStream)fileStream).Name).Name
                FileDownloadName = fileName
            };
        }

        #region Private Methods

        #endregion
    }
}
