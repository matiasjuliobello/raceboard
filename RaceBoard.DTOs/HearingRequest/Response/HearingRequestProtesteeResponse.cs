using RaceBoard.DTOs.Boat.Response;

namespace RaceBoard.DTOs.HearingRequest.Response
{
    public class HearingRequestProtesteeResponse
    {
        public int Id { get; set; }
        public BoatResponse Boat { get; set; }
    }
}
