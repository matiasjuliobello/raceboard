namespace RaceBoard.Domain
{
    public class TeamBoat
    {
        public int Id { get; set; }
        public Team Team { get; set; }
        public Boat Boat { get; set; }
    }
}
