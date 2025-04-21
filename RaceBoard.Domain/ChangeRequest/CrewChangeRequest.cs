namespace RaceBoard.Domain
{
    public class CrewChangeRequest : ChangeRequest
    {
        public User ReplacedUser { get; set; }
        public Person ReplacedPerson { get; set; }
        public string ReplacementFullName { get; set; }
    }
}