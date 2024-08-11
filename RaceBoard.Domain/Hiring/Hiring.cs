namespace RaceBoard.Domain
{
    public class Hiring
    {
        public int Id { get; set; }
        public User User { get; set; }
        public HiringType Type { get; set; }
        public Role Role { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
    }
}