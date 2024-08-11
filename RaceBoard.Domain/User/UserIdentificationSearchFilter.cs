namespace RaceBoard.Domain
{
    public class UserIdentificationSearchFilter
    {
        public int? Id { get; set; }
        public User? User { get; set; }
        public IdentificationType? Type { get; set; }
    }
}
