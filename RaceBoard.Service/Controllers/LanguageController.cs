using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;
using RaceBoard.DTOs.Language.Response;
using RaceBoard.Business.Managers.Interfaces;
using Language = RaceBoard.Domain.Language;

namespace RaceBoard.Service.Controllers
{
    [Route("api/languages")]
    [ApiController]
    public class LanguageController : AbstractController<LanguageController>
    {
        private readonly ILanguageManager _languageManager;

        public LanguageController
            (
                IMapper mapper,
                ILogger<LanguageController> logger,
                ITranslator translator,
                ILanguageManager languageManager,
                ISessionHelper sessionHelper,
                IRequestContextManager requestContextManager
            ) : base(mapper, logger, translator, sessionHelper, requestContextManager)
        {
            _languageManager = languageManager;
        }

        [HttpGet()]
        public ActionResult<List<LanguageResponse>> Get()
        {
            var data = _languageManager.Get();

            var response = _mapper.Map<List<Language>, List<LanguageResponse>>(data);

            return Ok(response);
        }

        #region Private Methods

        #endregion
    }
}

