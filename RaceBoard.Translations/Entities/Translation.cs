namespace RaceBoard.Translations.Entities
{
    public class Translation
    {
        public string Key { get; set; }
        public List<TranslatedText> Translations { get; set; }

        public Translation()
        {
            this.Translations = new List<TranslatedText>();
        }
    }
}