namespace RaceBoard.Domain
{
    public class ChampionshipMemberSearchFilter
    {
        public int[]? Ids { get; set; }
        public Championship? Championship { get; set; }
        public Role? Role { get; set; }
        public User? User { get; set; }
        public bool? IsActive { get; set; }
    }
}