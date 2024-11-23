namespace RaceBoard.DTOs.Team.Request
{
    public class TeamCheckRequest
    {
        //public int Id { get; set; }
        //public int IdTeamContestant { get; set; }
        public int IdPerson { get; set; }
        public int IdCompetition { get; set; }
        public int IdCheckType { get; set; }
    }
}
