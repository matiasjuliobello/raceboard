namespace RaceBoard.Domain
{
    public class PrivacyPolicy : AbstractEntity
    {
        public int VersionNumber { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTimeOffset CreationDate { get; set; }
    }
}