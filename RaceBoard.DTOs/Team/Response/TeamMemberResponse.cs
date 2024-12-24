using RaceBoard.DTOs.TeamMemberRole.Response;
using RaceBoard.DTOs.Person.Response;

namespace RaceBoard.DTOs.Team.Response
{
    public class TeamMemberResponse
    {
        public int Id { get; set; }
        public PersonSimpleResponse Person { get; set; }
        public TeamSimpleResponse Team {  get; set; }
        public TeamMemberRoleResponse Role { get; set; }
    }
}