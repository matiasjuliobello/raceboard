namespace RaceBoard.Domain
{
    public class HearingRequest
    {
        public int Id { get; set; }
        public int RequestNumber { get; set; }
        public Team Team { get; set; }
        public User RequestUser { get; set; }
        public Person RequestPerson { get; set; }
        public int IdRequestStatus { get; set; }
        public HearingRequestStatus Status { get; set; }
        public int IdHearingRequestType { get; set; }
        public HearingRequestType Type { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public string RaceNumber { get; set; }
        public HearingRequestProtestor Protestor { get; set; }
        public HearingRequestProtestees Protestees { get; set; }
        public HearingRequestIncident Incident { get; set; }
        public CommitteeBoatReturn CommitteeBoatReturn { get; set; }

        public HearingRequestWithdrawal Withdrawal {  get; set; }
        public HearingRequestLodgement Lodgement { get; set; }
        public HearingRequestAttendees Attendees { get; set; }
        public HearingRequestValidity Validity { get; set; }
        public HearingRequestResolution Resolution {  get; set; }
    }
}