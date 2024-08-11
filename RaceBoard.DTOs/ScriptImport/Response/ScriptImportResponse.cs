using RaceBoard.DTOs.Script.Response;
using RaceBoard.DTOs.User.Response;

namespace RaceBoard.DTOs.ScriptImport.Response
{
    public class ScriptImportResponse
    {
        public int Id { get; set; }
        public ScriptSimpleResponse Script { get; set; }
        public UserSimpleResponse CreationUser { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public List<ScriptImportFileResponse> Files { get; set; }
    }
}