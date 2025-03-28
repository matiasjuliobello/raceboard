using RaceBoard.DTOs.Boat.Request;

namespace RaceBoard.DTOs.HearingRequest.Request
{
    public class HearingRequestProtesteeRequest
    {
        public int Id { get; set; }
        public BoatRequest Boat { get; set; }
    }
}