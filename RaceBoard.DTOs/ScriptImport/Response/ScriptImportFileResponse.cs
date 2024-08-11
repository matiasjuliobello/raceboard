using RaceBoard.DTOs.File.Response;

namespace RaceBoard.DTOs.ScriptImport.Response
{
    public class ScriptImportFileResponse
    {
        public int Id { get; set; }
        public FileResponse File { get; set; }
        public ScriptImportFileTypeResponse FileType { get; set; }
    }
}
