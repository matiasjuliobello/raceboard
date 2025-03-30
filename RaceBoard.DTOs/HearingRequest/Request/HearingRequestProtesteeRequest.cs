using RaceBoard.DTOs.Team.Request;

namespace RaceBoard.DTOs.HearingRequest.Request
{
    public class HearingRequestProtesteeRequest
    {
        public int Id { get; set; }
        public TeamBoatRequest TeamBoat { get; set; }
    }
}