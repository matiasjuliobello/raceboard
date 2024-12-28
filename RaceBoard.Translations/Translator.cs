using RaceBoard.Common;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Translations.Entities;
using RaceBoard.Translations.Interfaces;
using Microsoft.Extensions.Configuration;

namespace RaceBoard.Translations
{
    public class Translator : ITranslator
    {
        private const string _DEFAULT_LANGUAGE = CommonValues.SystemDefaults.Culture;

        private string _language;

        private readonly ITranslationProvider _translationProvider;

        public Translator
            (
                ITranslationProvider translationProvider, 
                IHttpHeaderHelper httpHeaderHelper,
                IConfiguration configuration
            )
        {
            _translationProvider = translationProvider;

            string defaultLanguage = configuration["Translator_DefaultLanguage"] ?? _DEFAULT_LANGUAGE;

            var context = httpHeaderHelper.GetContext();
            if (context == null || String.IsNullOrEmpty(context.Language))
                SetCurrentLanguage(defaultLanguage);
            else
                SetCurrentLanguage(context.Language);
        }

        #region ITranslator implementation

        public string CurrentLanguage
        {
            get { return _language; }
        }

        public void SetCurrentLanguage(string language)
        {
            _language = language;
        }

        public string Get(string key, params object[] arguments)
        {
            TranslatedText translatedText = _translationProvider.Get(key, _language);
            if (translatedText == null)
                return key;

            string text = translatedText.Text;

            if (arguments != null && arguments.Length > 0)
                text = String.Format(text, arguments);

            return text;
        }

        #endregion
    }
}