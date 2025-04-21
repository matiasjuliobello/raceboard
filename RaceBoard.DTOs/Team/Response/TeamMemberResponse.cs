using RaceBoard.DTOs.TeamMemberRole.Response;
using RaceBoard.DTOs.Person.Response;
//using RaceBoard.DTOs.Permissions.Response;
using RaceBoard.DTOs.User.Response;

namespace RaceBoard.DTOs.Team.Response
{
    public class TeamMemberResponse
    {
        public int Id { get; set; }
        public TeamSimpleResponse Team { get; set; }
        public UserSimpleResponse User { get; set; }
        public PersonResponse Person { get; set; }
        //public RoleResponse Role { get; set; }
        public TeamMemberRoleResponse Role { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset JoinDate { get; set; }
    }
}