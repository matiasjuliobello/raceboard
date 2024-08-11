namespace RaceBoard.Domain
{
    public class RolePermissions
    {
        public Role Role { get; set; }
        public List<ActionRole> Permissions { get; set; }
    }
}