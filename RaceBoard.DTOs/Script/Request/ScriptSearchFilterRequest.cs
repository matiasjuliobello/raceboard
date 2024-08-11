namespace RaceBoard.DTOs.Script.Request
{
    public class ScriptSearchFilterRequest
    {
        public int? IdScript {  get; set; }
        public int? IdProject { get; set; }
        public int? IdStatus { get; set; }
        public string? Title { get; set; }
        public int? Episode { get; set; }
        public int? IdCreationUser { get; set; }
        public DateTimeOffset? CreationDate {  get; set; }
        public bool? HasImport { get; set; }
        public bool? HasPendingApproval { get; set; }
    }
}
