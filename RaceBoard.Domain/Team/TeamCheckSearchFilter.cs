using RaceBoard.Domain.Enums;

namespace RaceBoard.Domain
{
    public class TeamCheckSearchFilter
    {
        public int[]? Ids { get; set; }
        public Competition? Competition { get; set; }
        public List<RaceClass>? RaceClasses { get; set; }
        public Team? Team { get; set; }
        public TeamMemberCheckType? CheckType { get; set; }
        public DateTimeOffset? DateFrom { get; set; }
        public DateTimeOffset? DateTo { get; set; }
    }
}