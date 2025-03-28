namespace RaceBoard.Domain
{
    public class HearingRequest
    {
        public int Id { get; set; }
        public Team Team { get; set; }
        public User RequestUser { get; set; }
        public Person RequestPerson { get; set; }
        public HearingRequestStatus Status { get; set; }    // public Enums.RequestStatus Status { get; set; }
        public HearingRequestType Type { get; set; }        // public Enums.HearingRequestType Type { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public string RaceNumber { get; set; }
        public HearingRequestProtestor Protestor { get; set;}
        public HearingRequestProtestees Protestees { get; set; }
        public HearingRequestIncident Incident { get; set; }
    }
}