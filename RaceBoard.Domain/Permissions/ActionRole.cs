namespace RaceBoard.Domain
{
    public class ActionRole : AbstractEntity
    {
        public Action Action { get; set; }
        public Role Role { get; set; }
        public AuthorizationCondition Condition { get; set; }
    }
}
