using RaceBoard.DTOs.Boat.Response;
using RaceBoard.DTOs.RaceClass.Response;

namespace RaceBoard.DTOs.Team.Response
{
    public class TeamSimpleResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public RaceClassResponse RaceClass { get; set; }
        //public BoatResponse Boat { get; set; }
        //public List<TeamMemberResponse> Members { get; set; }
    }
}