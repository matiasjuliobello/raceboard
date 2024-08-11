namespace RaceBoard.Domain
{
    public class ScriptImportFile
    {
        public int Id { get; set; }
        public ScriptImport ScriptImport { get; set; }
        public File File { get; set; }
        public ScriptImportFileType FileType { get; set; } 
    }
}
