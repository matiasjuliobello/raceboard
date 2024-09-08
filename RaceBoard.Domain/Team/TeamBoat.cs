namespace RaceBoard.Domain
{
    public class TeamBoat : AbstractEntity
    {
        public Team Team { get; set; }
        public Boat Boat { get; set; }
    }
}
