using RaceBoard.DTOs.Invitation.Response;
//using RaceBoard.DTOs.Permissions.Response;
using RaceBoard.DTOs.Person.Response;
using RaceBoard.DTOs.TeamMemberRole.Response;
using RaceBoard.DTOs.User.Response;

namespace RaceBoard.DTOs.Team.Response
{
    public class TeamMemberInvitationResponse
    {
        public TeamResponse Team { get; set; }
        //public RoleResponse Role { get; set; }
        public TeamMemberRoleResponse Role { get; set; }
        public UserSimpleResponse RequestUser { get; set; }
        public UserSimpleResponse? User { get; set; }
        public PersonSimpleResponse? Person { get; set; }
        public DateTimeOffset RequestDate { get; set; }
        public bool IsPending { get; set; }
        public InvitationResponse Invitation { get; set; }
    }
}