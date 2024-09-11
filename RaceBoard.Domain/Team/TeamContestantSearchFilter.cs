namespace RaceBoard.Domain
{
    public class TeamContestantSearchFilter
    {
        public int[]? Ids { get; set; }
        public Team? Team { get; set; }
        public Person? Contestant { get; set; }
        public ContestantRole? Role { get; set; }
    }
}
