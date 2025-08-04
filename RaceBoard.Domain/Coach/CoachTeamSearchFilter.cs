namespace RaceBoard.Domain
{
    public class CoachTeamSearchFilter
    {
        public int[]? Ids { get; set; }
        public Coach? Coach { get; set; }
        public Team? Team { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public bool? IsActive { get; set; }
    }
}