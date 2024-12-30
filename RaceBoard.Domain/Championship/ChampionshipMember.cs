namespace RaceBoard.Domain
{
    public class ChampionshipMember
    {
        public int Id { get; set; }
        public Championship Championship { get; set; }
        public User User { get; set; }
        public Person Person { get; set; }
        public Role Role { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset JoinDate { get; set; }
    }
}