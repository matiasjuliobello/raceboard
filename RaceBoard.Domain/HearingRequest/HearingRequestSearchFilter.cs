namespace RaceBoard.Domain
{
    public class HearingRequestSearchFilter
    {
        public int[]? Ids { get; set; }
        public Championship? Championship { get; set; }
        public Team? Team { get; set; }
        public Enums.RequestStatus? Status { get; set; }
        public Enums.HearingRequestType? Type { get; set; }
        public DateTimeOffset? CreationDate { get; set; }
        public DateTimeOffset? ResolutionDate { get; set; }
        public User? RequestUser { get; set; }
        public string? RaceNumber { get; set; }
        public HearingRequestProtestor? Protestor { get; set; }
        public HearingRequestProtestees? Protestees { get; set; }
    }
}