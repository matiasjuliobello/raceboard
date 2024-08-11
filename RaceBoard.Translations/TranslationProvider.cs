using RaceBoard.Translations.Interfaces;
using RaceBoard.Common.Helpers.Interfaces;
using RaceBoard.Translations.Entities;

namespace RaceBoard.Translations
{
    public class TranslationProvider : ITranslationProvider
    {
        #region Private Methods

        private readonly ICacheHelper _cacheHelper;
        private readonly ITranslationRepository _translationRepository;

        private const string _CACHE_PREFIX = "TAG_";

        #endregion

        #region Constructors

        public TranslationProvider(ICacheHelper cacheHelper, ITranslationRepository translationRepository)
        {
            _cacheHelper = cacheHelper;
            _translationRepository = translationRepository;

            Initialize();
        }

        #endregion

        #region ITranslationProvider implementation

        public List<Translation> Get(string language)
        {
            var translations = GetFromRepository();

            foreach(var translation in translations)
            {
                translation.Translations.RemoveAll(x => x.Language != language);
            }

            return translations;
        }

        public TranslatedText Get(string key, string language)
        {
            var translation = GetFromCache(key);

            if (IsEmpty(translation))
            {
                translation = GetFromRepository(key);
                if (IsEmpty(translation))
                    return CreateDefault(key, language);

                SaveToCache(key, translation);
            }
            
            var translatedText = translation.Translations.FirstOrDefault(x => x.Language == language);
            if (translatedText == null)
            {
                translation = GetFromRepository(key);
                if (IsEmpty(translation))
                    return CreateDefault(key, language);

                SaveToCache(key, translation);
            }

            return translatedText;
        }

        #endregion

        #region Private Methods

        private void Initialize()
        {
            PopulateCacheIfEmpty();
        }

        private void PopulateCacheIfEmpty()
        {
            string key = "RecordNotFound";
            var trans = _cacheHelper.Get<Translation>($"{_CACHE_PREFIX}_{key}");
            if (trans != null)
                return;

            var translations = GetFromRepository();
            foreach (var translation in translations)
            {
                SaveToCache(translation.Key, translation);
            }
        }

        private Translation GetFromCache(string key)
        {
            return _cacheHelper.Get<Translation>($"{_CACHE_PREFIX}_{key}");
        }

        private void SaveToCache(string key, Translation translation)
        {
            _cacheHelper.Set<Translation>($"{_CACHE_PREFIX}_{key}", translation);
        }

        private bool IsEmpty(Translation translation)
        {
            return translation == null || translation.Translations.Count == 0;
        }

        private TranslatedText CreateDefault(string key, string language)
        {
            return new TranslatedText()
            {
                Language = language,
                Text = key
            };
        }

        private List<Translation> GetFromRepository()
        {
            return _translationRepository.Get();
        }

        private Translation GetFromRepository(string key)
        {
            return _translationRepository.Get(key, language: null);
        }

        #endregion
    }
}