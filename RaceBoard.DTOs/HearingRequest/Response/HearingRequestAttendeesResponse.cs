namespace RaceBoard.DTOs.HearingRequest.Response
{
    public class HearingRequestAttendeesResponse
    {
        public int Id { get; set; }
        public string Protestors { get; set; }
        public string Protestees { get; set; }
        public string Witnesses { get; set; }
        public string Interpreters { get; set; }
    }
}