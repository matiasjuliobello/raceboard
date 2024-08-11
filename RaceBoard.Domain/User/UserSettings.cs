namespace RaceBoard.Domain
{
    public class UserSettings
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Culture Culture { get; set; }
        public TimeZone TimeZone { get; set; }
        public Language Language { get; set; }
    }
}
