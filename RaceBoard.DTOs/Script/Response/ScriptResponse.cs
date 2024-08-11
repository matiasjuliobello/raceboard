using RaceBoard.DTOs.Language.Response;
using RaceBoard.DTOs.Project.Response;
using RaceBoard.DTOs.User.Response;

namespace RaceBoard.DTOs.Script.Response
{
    public class ScriptResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Episode { get; set; }
        public string Comments { get; set; }
        public ProjectSimpleResponse Project { get; set; }
        public ScriptStatusResponse Status { get; set; }
        public UserSimpleResponse CreationUser { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public int RunningTime { get; set; }
        public int Pages { get; set; }
        public int Loops { get; set; }
        public int Characters { get; set; }
        public LanguageResponse OriginalLanguage { get; set; }
        public LanguageResponse DubbingLanguage { get; set; }
        public bool IsActive { get; set; }
        public bool HasImport { get; set; }
        public bool HasPendingApproval { get; set; }
    }
}
