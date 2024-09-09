namespace RaceBoard.DTOs.Team.Request
{
    public class TeamRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int IdCompetition { get; set; }
        public int IdRaceClass { get; set; }
    }
}