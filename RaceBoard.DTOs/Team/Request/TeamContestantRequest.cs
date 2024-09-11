namespace RaceBoard.DTOs.Team.Request
{
    public class TeamContestantRequest
    {
        public int Id { get; set; }
        public int IdTeam { get; set; }
        public int IdPerson { get; set; }
        public int IdContestantRole { get; set; }
    }
}
