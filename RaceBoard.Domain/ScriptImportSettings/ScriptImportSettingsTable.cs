namespace RaceBoard.Domain
{
    public class ScriptImportSettingsTable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public short Index { get; set; }
        public bool SkipFirstRow { get; set; }
        public List<ScriptImportSettingsTableLayout> Layouts { get; set; }

        public ScriptImportSettingsTable()
        {
            this.Layouts = new List<ScriptImportSettingsTableLayout>();
        }
    }
}