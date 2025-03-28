namespace RaceBoard.DTOs.HearingRequest.Request
{
    public class HearingRequestIncidentRequest
    {
        public int Id { get; set; }
        public string Time { get; set; }
        public string Place { get; set; }
        public string BrokenRules { get; set; }
        public string Witnesses { get; set; }
        public string Details { get; set; }
    }
}