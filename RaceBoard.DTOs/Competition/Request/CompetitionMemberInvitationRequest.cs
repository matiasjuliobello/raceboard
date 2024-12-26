namespace RaceBoard.DTOs.Competition.Request
{
    public class CompetitionMemberInvitationRequest
    {
        public int Id { get; set; }
        public int IdCompetition { get; set; }
        public int IdRole { get; set; }
        public int? IdUser { get; set; }

        public InvitationRequest Invitation { get; set; }
    }
}