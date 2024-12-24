namespace RaceBoard.DTOs.Team.Request
{
    public class TeamMemberRequest
    {
        public int Id { get; set; }
        public int IdTeam { get; set; }
        public int IdPerson { get; set; }
        public int IdTeamMemberRole { get; set; }
    }
}
