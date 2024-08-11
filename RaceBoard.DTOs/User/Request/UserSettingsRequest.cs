namespace RaceBoard.DTOs.User.Request
{
    public class UserSettingsRequest
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public int IdCulture { get; set; }
        public int IdLanguage { get; set; }
        public int IdTimeZone { get; set; }
    }
}
