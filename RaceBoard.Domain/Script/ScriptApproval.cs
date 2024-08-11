namespace RaceBoard.Domain
{
    public class ScriptApproval
    {
        public int Id { get; set; }
        public Script Script { get; set; }
        public Role Role { get; set; }
        public User User { get; set; }
        public DateTimeOffset Date { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public string Comments { get; set; }
    }
}