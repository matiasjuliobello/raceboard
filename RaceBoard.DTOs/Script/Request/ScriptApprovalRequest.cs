namespace RaceBoard.DTOs.Script.Request
{
    public class ScriptApprovalRequest
    {
        public int Id { get; set; }
        public int IdScript { get; set; }
        public int IdRole { get; set; }
        public int IdUser { get; set; }
        public int IdStatusApproval { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Comments { get; set; }
    }
}