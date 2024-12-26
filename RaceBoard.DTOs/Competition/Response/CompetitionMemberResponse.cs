using RaceBoard.DTOs.Organization.Response;
using RaceBoard.DTOs.Permissions.Response;
using RaceBoard.DTOs.Person.Response;
using RaceBoard.DTOs.User.Response;

namespace RaceBoard.DTOs.Competition.Response
{
    public class CompetitionMemberResponse
    {
        public int Id { get; set; }
        public CompetitionResponse Competition { get; set; }
        public UserSimpleResponse User { get; set; }
        public PersonSimpleResponse Person { get; set; }
        public RoleResponse Role { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset JoinDate { get; set; }
    }
}