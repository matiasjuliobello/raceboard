namespace RaceBoard.DTOs
{
    public class InvitationRequest
    {
        public int Id { get; set; }
        public int IdRequest { get; set; }
        public string Token { get; set; }
        public string EmailAddress { get; set; }
        public bool IsExpired { get; set; }
    }
}