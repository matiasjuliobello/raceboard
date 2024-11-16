using RaceBoard.DTOs.ContestantRole.Response;
using RaceBoard.DTOs.Person.Response;

namespace RaceBoard.DTOs.Team.Response
{
    public class TeamContestantResponse
    {
        public int Id { get; set; }
        public PersonSimpleResponse Person { get; set; }
        public TeamSimpleResponse Team {  get; set; }
        public ContestantRoleResponse Role { get; set; }
    }
}