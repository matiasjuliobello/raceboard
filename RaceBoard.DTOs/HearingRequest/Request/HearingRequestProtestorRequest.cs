using RaceBoard.DTOs.Team.Request;

namespace RaceBoard.DTOs.HearingRequest.Request
{
    public class HearingRequestProtestorRequest
    {
        public int Id { get; set; }
        public TeamBoatRequest TeamBoat { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public HearingRequestProtestorNoticeRequest Notice { get; set; }
    }
}