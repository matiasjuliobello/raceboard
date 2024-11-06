namespace RaceBoard.Domain
{
    public class RaceProtest
    {
        public int Id { get; set; }
        public Race Race { get; set; }
        public TeamContestant TeamContestant { get; set; }
        public DateTimeOffset Submission { get; set; }
    }
}