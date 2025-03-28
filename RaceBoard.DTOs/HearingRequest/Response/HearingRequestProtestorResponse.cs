using RaceBoard.DTOs.Boat.Response;

namespace RaceBoard.DTOs.HearingRequest.Response
{
    public class HearingRequestProtestorResponse
    {
        public int Id { get; set; }
        public BoatResponse Boat { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public HearingRequestProtestorNoticeResponse Notice { get; set; }
    }
}