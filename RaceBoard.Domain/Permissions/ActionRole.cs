namespace RaceBoard.Domain
{
    public class ActionRole
    {
        public int Id { get; set; }
        public Action Action { get; set; }
        public Role Role { get; set; }
        public AuthorizationCondition Condition { get; set; }
    }
}
