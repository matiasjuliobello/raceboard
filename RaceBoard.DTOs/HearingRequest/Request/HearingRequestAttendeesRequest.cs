namespace RaceBoard.DTOs.HearingRequest.Request
{
    public class HearingRequestAttendeesRequest
    {
        public int Id { get; set; }
        public string Protestors { get; set; }
        public string Protestees { get; set; }
        public string Witnesses { get; set; }
        public string Interpreters { get; set; }
    }
}
