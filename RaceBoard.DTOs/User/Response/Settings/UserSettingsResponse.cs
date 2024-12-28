using RaceBoard.DTOs.Format.Response;
using RaceBoard.DTOs.Language.Response;

namespace RaceBoard.DTOs.User.Response.Settings
{
    public class UserSettingsResponse
    {
        public int Id { get; set; }
        public UserSimpleResponse User { get; set; }
        public LanguageResponse Language { get; set; }
        public TimeZoneResponse TimeZone { get; set; }
        public DateFormatResponse DateFormat { get; set; }
    }
}
