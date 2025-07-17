using RaceBoard.DTOs.Championship.Response;
using RaceBoard.DTOs.Person.Response;
using RaceBoard.DTOs.Team.Response;
using RaceBoard.DTOs.User.Response;

namespace RaceBoard.DTOs.HearingRequest.Response
{
    public class HearingRequestResponse
    {
        public int Id { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public int RequestNumber { get; set; }
        public string RaceNumber { get; set; }
        public HearingRequestStatusResponse Status { get; set; }
        public HearingRequestTypeResponse Type { get; set; }
        public TeamSimpleResponse Team { get; set; }
        public UserSimpleResponse RequestUser { get; set; }
        public PersonSimpleResponse RequestPerson { get; set; }
        public HearingRequestProtestorResponse Protestor { get; set; }
        public HearingRequestProtesteesResponse Protestees { get; set; }
        public HearingRequestIncidentResponse Incident { get; set; }
        public ChampionshipCommitteeBoatReturnResponse CommitteeBoatReturn { get; set; }

        public HearingRequestWithdrawalResponse Withdrawal { get; set; }
        public HearingRequestLodgementResponse Lodgement { get; set; }
        public HearingRequestAttendeesResponse Attendees { get; set; }
        public HearingRequestValidityResponse Validity { get; set; }
        public HearingRequestResolutionResponse Resolution { get; set; }
    }
}