using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RaceBoard.Service.Controllers.Abstract;
using RaceBoard.Service.Helpers.Interfaces;
using RaceBoard.Translations.Interfaces;
using RaceBoard.DTOs.Translation.Response;
using Microsoft.AspNetCore.Authorization;
using RaceBoard.Business.Managers.Interfaces;


namespace RaceBoard.Service.Controllers
{
    [Route("api/translations")]
    [ApiController]
    public class TranslationController : AbstractController<TranslationController>
    {
        private readonly ITranslationProvider _translationProvider;
        private readonly ITranslator _translator;

        public TranslationController
            (
                IMapper mapper,
                ILogger<TranslationController> logger,
                ITranslator translator,
                ITranslationProvider translationProvider,
                ISessionHelper sessionHelper,
                IRequestContextManager requestContextManager
            ) : base(mapper, logger, translator, sessionHelper, requestContextManager)
        {
            _translator = translator;
            _translationProvider = translationProvider;
        }

        [HttpGet()]
        [AllowAnonymous]
        public ActionResult<List<TranslationResponse>> Get()
        {
            string language = base.GetRequestLanguage(); //_translator.CurrentLanguagev
            if (string.IsNullOrEmpty(language))
                language = "en-US";

            var data = _translationProvider.Get(language);

            var response = _mapper.Map<List<TranslationResponse>>(data);

            return Ok(response);
        }


        #region Private Methods


        #endregion
    }
}