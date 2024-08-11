namespace RaceBoard.Domain
{
    public class ScriptImportSettingsTableLayout
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string Field { get; set; }
        public ScriptImportSettingsTable Table {  get; set; }
        public int IdTable { get; set; }
        public short Row { get; set; }
        public short Column { get; set; }
    }
}
