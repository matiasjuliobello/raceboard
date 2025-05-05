namespace RaceBoard.Domain
{
    public class HearingRequestLodgement
    {
        public int Id {  get; set; }
        public HearingRequest HearingRequest { get; set; }
        public TimeSpan? Deadline { get; set; }
        public bool IsInTerm { get; set; }
        public bool HasExtension { get; set; }
    }
}
