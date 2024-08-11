namespace RaceBoard.DTOs.Script.Request
{
    public class ScriptRequest
    {
        public int Id { get; set; }
        public int IdProject { get; set; }
        public int IdStatus { get; set; }
        public string Title { get; set; }
        public int Episode { get; set; }
        public string Comments { get; set; }
        public int RunningTime { get; set; }
        public int Pages { get; set; }
        public int Loops { get; set; }
        public int Characters { get; set; }
        public int IdOriginalLanguage { get; set; }
        public int IdDubbingLanguage { get; set; }
    }
}