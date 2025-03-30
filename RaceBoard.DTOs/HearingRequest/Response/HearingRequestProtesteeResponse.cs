using RaceBoard.DTOs.Team.Response;

namespace RaceBoard.DTOs.HearingRequest.Response
{
    public class HearingRequestProtesteeResponse
    {
        public int Id { get; set; }
        public TeamBoatResponse TeamBoat { get; set; }
    }
}
