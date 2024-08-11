namespace RaceBoard.Domain
{
    public class HiringSearchFilter
    {
        public User? Provider { get; set; }
        public HiringType? Type { get; set; }
        public Role[]? Roles { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
    }
}