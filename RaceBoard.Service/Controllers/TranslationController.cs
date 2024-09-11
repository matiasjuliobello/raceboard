using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;
using RaceBoard.DTOs.Translation.Response;
using Microsoft.AspNetCore.Authorization;

namespace RaceBoard.Service.Controllers
{
    [Route("api/translations")]
    [ApiController]
    public class TranslationController : AbstractController<TranslationController>
    {
        private readonly ITranslationProvider _translationProvider;

        public TranslationController
            (
                IMapper mapper,
                ILogger<TranslationController> logger,
                ITranslator translator,
                ITranslationProvider translationProvider,
                ISessionHelper sessionHelper,
                IRequestContextHelper requestContextHelper
            ) : base(mapper, logger, translator, sessionHelper, requestContextHelper)
        {
            _translationProvider = translationProvider;
        }

        [HttpGet()]
        [AllowAnonymous]
        public ActionResult<List<TranslationResponse>> Get()
        {
            string language = "es-AR";

            var data = _translationProvider.Get(language);

            var response = _mapper.Map<List<TranslationResponse>>(data);

            return Ok(response);
        }


        #region Private Methods


        #endregion
    }
}