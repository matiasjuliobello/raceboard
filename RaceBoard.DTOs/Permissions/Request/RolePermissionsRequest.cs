namespace RaceBoard.DTOs.Permissions.Request
{
    public class RolePermissionsRequest
    {
        public int IdRole {  get; set; }
        public ActionRoleRequest[] Permissions { get; set; }
    }
}
