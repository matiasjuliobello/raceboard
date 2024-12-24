namespace RaceBoard.DTOs.Invitation.Response
{
    public class InvitationResponse
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string EmailAddress { get; set; }
        public bool IsExpired { get; set; }
    }
}