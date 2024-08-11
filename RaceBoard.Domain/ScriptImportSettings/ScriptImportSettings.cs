namespace RaceBoard.Domain
{
    public class ScriptImportSettings
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ScriptImportSettingsTable> Tables { get; set; }
        public List<ScriptImportSettingsValidation> Validations { get; set; }
        public List<ScriptImportSettingsAnnotation> Annotations { get; set; }
        public List<ScriptImportSettingsLoopCountMethod> LoopCountMethods { get; set; }
    }
}