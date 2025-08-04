using RaceBoard.DTOs.Permissions.Response;

namespace RaceBoard.DTOs.User.Response
{
    public class UserRoleResponse
    {
        public int Id { get; set; }
        //public UserResponse User { get; set; }
        public RoleResponse Role { get; set; }
    }
}
