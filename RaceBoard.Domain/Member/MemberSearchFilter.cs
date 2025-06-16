namespace RaceBoard.Domain
{
    public class MemberSearchFilter
    {
        public Championship Championship { get; set; }
        public Organization? Organization { get; set; }
        public User? User { get; set; }
        public Person? Person { get; set; }
        public Team? Team { get; set; }
        public RaceClass[]? RaceClasses { get; set; }
    }
}