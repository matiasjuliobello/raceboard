using RaceBoard.DTOs.Boat.Response;
using RaceBoard.DTOs.Competition.Response;
using RaceBoard.DTOs.Organization.Response;
using RaceBoard.DTOs.RaceClass.Response;

namespace RaceBoard.DTOs.Team.Response
{
    public class TeamResponse
    {
        public int Id { get; set; }
        public OrganizationResponse Organization { get; set; }
        public CompetitionSimpleResponse Competition { get; set; }
        public RaceClassResponse RaceClass { get; set; }
        public BoatResponse Boat { get; set; }
        public List<TeamMemberResponse> Members { get; set;}
    }
}
