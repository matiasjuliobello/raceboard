namespace RaceBoard.Domain
{
    public class RolePermissions : AbstractEntity
    {
        public Role Role { get; set; }
        public List<ActionRole> Permissions { get; set; }
    }
}