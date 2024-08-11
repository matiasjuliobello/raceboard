using RaceBoard.DTOs.Permissions.Response;
using RaceBoard.DTOs.User.Response;

namespace RaceBoard.DTOs.Hiring.Response
{
    public class HiringResponse
    {
        public int Id { get; set; }
        public UserSimpleResponse User { get; set; }
        public HiringTypeResponse Type { get; set; }
        public RoleResponse Role { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
    }
}