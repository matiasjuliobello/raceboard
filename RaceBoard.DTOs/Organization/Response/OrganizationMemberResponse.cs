using RaceBoard.DTOs.Invitation.Response;
using RaceBoard.DTOs.Permissions.Response;
using RaceBoard.DTOs.Person.Response;
using RaceBoard.DTOs.User.Response;

namespace RaceBoard.DTOs.Organization.Response
{
    public class OrganizationMemberResponse
    {
        public int Id { get; set; }
        public OrganizationResponse Organization { get; set; }
        public UserSimpleResponse User { get; set; }
        public PersonSimpleResponse Person { get; set; }
        public RoleResponse Role { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset JoinDate { get; set; }
    }
}