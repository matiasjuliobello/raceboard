namespace RaceBoard.Domain
{
    public class Member
    {
        public Championship Championship { get; set; }
        public User User { get; set; }
        public Person Person { get; set; }
        public Team? Team { get; set; }
    }
}