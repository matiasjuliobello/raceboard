namespace RaceBoard.DTOs.Project.Response
{
    public class ProjectSimpleResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ProjectTypeResponse Type { get; set; }
    }
}