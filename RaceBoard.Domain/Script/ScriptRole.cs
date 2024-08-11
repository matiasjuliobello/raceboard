namespace RaceBoard.Domain
{
    public class ScriptRole
    {
        public int Id { get; set; }
        public Script Script { get; set; }
        public Role Role { get; set; }
        public User User { get; set; }
    }
}