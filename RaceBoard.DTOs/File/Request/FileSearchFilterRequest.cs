using RaceBoard.DTOs.User.Response;

namespace RaceBoard.DTOs.File.Request
{
    public class FileSearchFilterRequest
    {
        public string? PhysicalName { get; set; }
        public UserSimpleResponse? CreationUser { get; set; }
        public DateTimeOffset? CreationDate { get; set; }
    }
}