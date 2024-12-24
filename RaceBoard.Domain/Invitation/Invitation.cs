namespace RaceBoard.Domain
{
    public class Invitation
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string EmailAddress { get; set; }
        public bool IsExpired { get; set; }
    }
}