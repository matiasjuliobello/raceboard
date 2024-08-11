namespace RaceBoard.DTOs.ScriptImportSettings.Response
{
    public class ScriptImportSettingsResponse
    {
        public int Id { get; set; }
        public List<ScriptImportSettingsTableResponse> Tables { get; set; }
        public List<ScriptImportSettingsValidationResponse> Validations { get; set; }
        public List<ScriptImportSettingsAnnotationResponse> Annotations { get; set; }
        public List<ScriptImportSettingsLoopCountMethodResponse> LoopCountMethods { get; set; }
    }
}