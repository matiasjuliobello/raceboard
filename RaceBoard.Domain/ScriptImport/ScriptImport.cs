namespace RaceBoard.Domain
{
    public class ScriptImport
    {
        public int Id { get; set; }
        public Script Script { get; set; }
        public User CreationUser { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public List<ScriptImportFile> Files { get; set; }
    }
}
