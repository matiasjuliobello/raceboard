namespace RaceBoard.Domain
{
    public class RaceComplaint
    {
        public int Id { get; set; }
        public Race Race { get; set; }
        public TeamContestant TeamContestant { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}