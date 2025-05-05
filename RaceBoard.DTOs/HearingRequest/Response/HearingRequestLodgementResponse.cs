namespace RaceBoard.DTOs.HearingRequest.Response
{
    public class HearingRequestLodgementResponse
    {
        public int Id { get; set; }
        public TimeSpan? Deadline { get; set; }
        public bool IsInTerm { get; set; }
        public bool HasExtension { get; set; }
    }
}