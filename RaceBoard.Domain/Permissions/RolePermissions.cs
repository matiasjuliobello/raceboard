namespace RaceBoard.Domain
{
    public class RolePermissions
    {
        public int Id { get; set; }
        public Role Role { get; set; }
        public List<ActionRole> Permissions { get; set; }
    }
}