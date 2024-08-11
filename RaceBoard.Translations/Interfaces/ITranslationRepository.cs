using RaceBoard.Translations.Entities;

namespace RaceBoard.Translations.Interfaces
{
    public interface ITranslationRepository
    {
        List<Translation> Get(string? language = null);
        Translation Get(string key, string? language = null);
    }
}