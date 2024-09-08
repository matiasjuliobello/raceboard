namespace RaceBoard.Domain
{
    public class UserPasswordReset : AbstractEntity
    {
        public User User { get; set; }
        public string Token { get; set; }
        public DateTimeOffset RequestDate { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }
        public DateTimeOffset? UseDate { get; set; }
        public bool IsUsed { get; set; }
        public bool IsActive { get; set; }
    }
}
