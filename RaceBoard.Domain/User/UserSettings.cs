namespace RaceBoard.Domain
{
    public class UserSettings : AbstractEntity
    {
        public User User { get; set; }
        public Culture Culture { get; set; }
        public TimeZone TimeZone { get; set; }
        public Language Language { get; set; }
    }
}
