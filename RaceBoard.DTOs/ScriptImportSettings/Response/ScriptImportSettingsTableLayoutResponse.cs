namespace RaceBoard.DTOs.ScriptImportSettings.Response
{
    public class ScriptImportSettingsTableLayoutResponse
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string Field { get; set; }
        public short Row { get; set; }
        public short Column { get; set; }
    }
}