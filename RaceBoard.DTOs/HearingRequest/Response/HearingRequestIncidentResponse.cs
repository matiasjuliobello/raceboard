namespace RaceBoard.DTOs.HearingRequest.Response
{
    public class HearingRequestIncidentResponse
    {
        public int Id { get; set; }
        public TimeSpan Time { get; set; }
        public string Place { get; set; }
        public string BrokenRules { get; set; }
        public string Witnesses { get; set; }
        public string Details { get; set; }
    }
}