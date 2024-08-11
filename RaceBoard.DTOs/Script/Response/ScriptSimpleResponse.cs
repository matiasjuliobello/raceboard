using RaceBoard.DTOs.Project.Response;

namespace RaceBoard.DTOs.Script.Response
{
    public class ScriptSimpleResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Episode { get; set; }
        public ProjectSimpleResponse Project { get; set; }
    }
}