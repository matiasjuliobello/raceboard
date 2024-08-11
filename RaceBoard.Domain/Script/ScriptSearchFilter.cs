namespace RaceBoard.Domain
{
    public class ScriptSearchFilter
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public int? Episode { get; set; }
        public DateTimeOffset? CreationDate { get; set; }
        public User? CreationUser { get; set; }
        public Project Project { get; set; }
        public ScriptStatus? Status { get; set; }
        public bool? HasImport { get; set; }
        public bool? HasPendingApproval { get; set; }
    }
}