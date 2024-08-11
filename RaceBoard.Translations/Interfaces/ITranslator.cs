namespace RaceBoard.Translations.Interfaces
{
    public interface ITranslator
    {
        string CurrentLanguage { get; }

        void SetCurrentLanguage(string language);
        string Get(string key, params object[] arguments);
    }
}