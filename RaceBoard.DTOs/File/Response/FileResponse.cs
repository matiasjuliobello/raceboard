using RaceBoard.DTOs.Person.Response;
using RaceBoard.DTOs.User.Response;

namespace RaceBoard.DTOs.File.Response
{
    public class FileResponse
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public UserSimpleResponse CreationUser { get; set; }
        public PersonSimpleResponse CreationPerson { get; set; }
        public DateTimeOffset CreationDate { get; set; }
    }
}
