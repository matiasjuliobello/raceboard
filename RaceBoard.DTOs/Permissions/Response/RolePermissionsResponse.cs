namespace RaceBoard.DTOs.Permissions.Response
{
    public class RolePermissionsResponse
    {
        public int IdRole {  get; set; }
        public List<ActionRoleResponse> Permissions {  get; set; }
    }
}
