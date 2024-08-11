using RaceBoard.DTOs.User.Response;

namespace RaceBoard.DTOs.Project.Response
{
    public class ProjectResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public ProjectTypeResponse Type { get; set; }
        public UserSimpleResponse CreationUser { get; set; }
        public DateTimeOffset CreationDate { get; set; }
    }
}