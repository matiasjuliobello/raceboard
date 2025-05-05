namespace RaceBoard.DTOs.HearingRequest.Request
{
    public class HearingRequestLodgementRequest
    {
        public int Id { get; set; }
        public string Deadline { get; set; }
        public bool IsInTerm { get; set; }
        public bool HasExtension { get; set; }
    }
}