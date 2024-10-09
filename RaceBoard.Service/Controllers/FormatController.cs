using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Business.Managers.Interfaces;
using RaceBoard.DTOs.Format.Response;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;

namespace RaceBoard.Service.Controllers
{
    [Route("api/formats")]
    [ApiController]
    public class FormatController : AbstractController<FormatController>
    {
        private readonly IFormatManager _formatManager;

        public FormatController
            (
                IMapper mapper,
                ILogger<FormatController> logger,
                ITranslator translator,
                IFormatManager FormatManager,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _formatManager = FormatManager;
        }

        [HttpGet("dates")]
        public ActionResult<List<DateFormatResponse>> Get()
        {
            var data = _formatManager.GetDateFormats();

            var response = _mapper.Map<List<DateFormatResponse>>(data);

            return Ok(response);
        }

        #region Private Methods

        #endregion
    }
}
