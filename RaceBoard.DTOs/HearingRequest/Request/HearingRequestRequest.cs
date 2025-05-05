namespace RaceBoard.DTOs.HearingRequest.Request
{
    public class HearingRequestRequest
    {
        public int Id { get; set; }
        public int IdTeam { get; set; }
        public int IdHearingRequestStatus { get; set; } // public Enums.RequestStatus Status { get; set; }
        public int IdHearingRequestType { get; set; }   // public Enums.HearingRequestType Type { get; set; }
        public int RequestNumber { get; set; }
        public string RaceNumber { get; set; }
        public HearingRequestProtestorRequest Protestor { get; set; }
        public HearingRequestProtesteesRequest Protestees { get; set; }
        public HearingRequestIncidentRequest Incident { get; set; }

        public HearingRequestWithdrawalRequest Withdrawal { get; set; }
        public HearingRequestLodgementRequest Lodgement { get; set; }
        public HearingRequestAttendeesRequest Attendees { get; set; }
        public HearingRequestValidityRequest Validity { get; set; }
        public HearingRequestResolutionRequest Resolution { get; set; }
    }
}