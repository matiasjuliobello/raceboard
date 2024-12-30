namespace RaceBoard.Domain
{
    public class TeamMemberSearchFilter
    {
        public int[]? Ids { get; set; }
        public Team? Team { get; set; }
        public Person? Member { get; set; }
        public User? User { get; set; }
        public TeamMemberRole? Role { get; set; }
        public bool? IsActive {  get; set; }
    }
}
