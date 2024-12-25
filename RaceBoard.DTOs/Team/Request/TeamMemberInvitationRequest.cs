namespace RaceBoard.DTOs.Team.Request
{
    public class TeamMemberInvitationRequest
    {
        public int Id { get; set; }
        public int IdTeam { get; set; }
        public int IdRole { get; set; }
        public int? IdUser { get; set; }

        public InvitationRequest Invitation { get; set; }
    }
}