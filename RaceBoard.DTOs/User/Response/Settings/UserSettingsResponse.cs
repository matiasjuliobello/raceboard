using RaceBoard.DTOs.Language.Response;

namespace RaceBoard.DTOs.User.Response.Settings
{
    public class UserSettingsResponse
    {
        public int Id { get; set; }
        //public UserSimpleResponse User { get; set; }
        public CultureResponse Culture { get; set; }
        public LanguageResponse Language { get; set; }
        public TimeZoneResponse TimeZone { get; set; }
    }
}
