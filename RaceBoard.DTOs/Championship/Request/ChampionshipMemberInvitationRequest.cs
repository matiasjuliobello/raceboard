namespace RaceBoard.DTOs.Championship.Request
{
    public class ChampionshipMemberInvitationRequest
    {
        public int Id { get; set; }
        public int IdChampionship { get; set; }
        public int IdRole { get; set; }
        public int? IdUser { get; set; }

        public InvitationRequest Invitation { get; set; }
    }
}