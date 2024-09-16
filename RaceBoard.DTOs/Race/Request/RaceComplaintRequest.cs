namespace RaceBoard.DTOs.Race.Request
{
    public class RaceComplaintRequest
    {
        public int Id { get; set; }
        public int IdRace { get; set; }
        public int IdTeamContestant { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}