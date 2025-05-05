namespace RaceBoard.Domain
{
    public class HearingRequestAttendees
    {
        public int Id { get; set; }
        public HearingRequest HearingRequest { get; set; }
        public string Protestors { get; set; }
        public string Protestees { get; set; }
        public string Witnesses { get; set; }
        public string Interpreters { get; set; }
    }
}
