namespace RaceBoard.DTOs.ScriptImportSettings.Response
{
    public class ScriptImportSettingsTableResponse
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public bool SkiptFirstRow { get; set; }
        public string Name { get; set; }
        public List<ScriptImportSettingsTableLayoutResponse> Layout { get; set; }
    }
}
