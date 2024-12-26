namespace RaceBoard.Domain
{
    public class CompetitionMemberSearchFilter
    {
        public int[]? Ids { get; set; }
        public Competition? Competition { get; set; }
        public Role? Role { get; set; }
        public User? User { get; set; }
    }
}