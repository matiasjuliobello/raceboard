namespace RaceBoard.DTOs.Race.Request
{
    public class RaceProtestRequest
    {
        public int Id { get; set; }
        public int IdRace { get; set; }
        public int IdTeamMember { get; set; }
        public DateTimeOffset Submission { get; set; }
    }
}