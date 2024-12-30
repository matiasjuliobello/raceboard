using RaceBoard.DTOs.Organization.Response;
using RaceBoard.DTOs.Permissions.Response;
using RaceBoard.DTOs.Person.Response;
using RaceBoard.DTOs.User.Response;

namespace RaceBoard.DTOs.Championship.Response
{
    public class ChampionshipMemberResponse
    {
        public int Id { get; set; }
        public ChampionshipResponse Championship { get; set; }
        public UserSimpleResponse User { get; set; }
        public PersonSimpleResponse Person { get; set; }
        public RoleResponse Role { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset JoinDate { get; set; }
    }
}