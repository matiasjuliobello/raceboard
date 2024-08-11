using RaceBoard.Translations.Entities;

namespace RaceBoard.Translations.Interfaces
{
    public interface ITranslationProvider
    {
        List<Translation> Get(string language);

        TranslatedText Get(string key, string language);
    }
}
