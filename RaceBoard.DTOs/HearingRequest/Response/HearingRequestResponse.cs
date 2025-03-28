using RaceBoard.DTOs.ChangeRequest.Response;
using RaceBoard.DTOs.Person.Response;
using RaceBoard.DTOs.Team.Response;
using RaceBoard.DTOs.User.Response;

namespace RaceBoard.DTOs.HearingRequest.Response
{
    public class HearingRequestResponse
    {
        public int Id { get; set; }
        public RequestStatusResponse Status { get; set; }
        public HearingRequestTypeResponse Type { get; set; }
        public TeamSimpleResponse Team { get; set; }
        public UserSimpleResponse RequestUser { get; set; }
        public PersonSimpleResponse RequestPerson { get; set; }
        public DateTimeOffset CreationDate { get; set; }
        public string RaceNumber { get; set; }
        public HearingRequestProtestorResponse Protestor { get; set; }
        public HearingRequestProtesteesResponse Protestees { get; set; }
    }
}