using RaceBoard.DTOs.Boat.Response;

namespace RaceBoard.DTOs.Team.Response
{
    public class TeamBoatResponse
    {
        public int Id { get; set; }
        public TeamResponse Team { get; set; }
        public BoatResponse Boat { get; set; }
    }
}