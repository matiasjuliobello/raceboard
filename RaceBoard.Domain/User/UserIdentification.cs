namespace RaceBoard.Domain
{
    public class UserIdentification
    {
        public int Id { get; set; }
        public User User { get; set; }
        public IdentificationType Type { get; set; }
        public string Number { get; set; }
        public bool IsMain { get; set; }
    }
}
