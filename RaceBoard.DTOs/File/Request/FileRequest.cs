using RaceBoard.DTOs.User.Response;

namespace RaceBoard.DTOs.File.Request
{
    public class FileRequest
    {
        public int Id { get; set; }
        public int IdFileType { get; set; }
        public string PhysicalName { get; set; }
        public UserSimpleResponse CreationUser { get; set; }
        public DateTimeOffset CreationDate { get; set; }
    }
}