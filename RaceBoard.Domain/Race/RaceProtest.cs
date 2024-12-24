namespace RaceBoard.Domain
{
    public class RaceProtest
    {
        public int Id { get; set; }
        public Race Race { get; set; }
        public TeamMember TeamMember { get; set; }
        public DateTimeOffset Submission { get; set; }
    }
}