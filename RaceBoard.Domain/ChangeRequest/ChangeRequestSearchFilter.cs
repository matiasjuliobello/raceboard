using RaceBoard.Domain.Enums;

namespace RaceBoard.Domain
{
    public class ChangeRequestSearchFilter
    {
        public int[]? Ids { get; set; }
        public Team? Team { get; set; }
        public Enums.RequestStatus? Status { get; set; }
        public DateTimeOffset? CreationDate { get; set; }
        public DateTimeOffset? ResolutionDate { get; set; }
    }
}