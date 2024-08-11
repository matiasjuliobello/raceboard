using RaceBoard.DTOs.User.Response;

namespace RaceBoard.DTOs.File.Response
{
    public class FileResponse
    {
        public int Id { get; set; }
        public string PhysicalName { get; set; }
        public UserSimpleResponse CreationUser { get; set; }
        public DateTimeOffset CreationDate { get; set; }
    }
}
